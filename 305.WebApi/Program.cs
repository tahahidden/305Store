using System.IO.Compression;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.IService;
using _305.BuildingBlocks.Configurations;
using _305.BuildingBlocks.IService;
using _305.BuildingBlocks.Service;
using _305.BuildingBlocks.Helper;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.Service;
using _305.WebApi.Assistants.Middleware;
using _305.WebApi.Assistants.Permission;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// ─────────────── Logger (Serilog) ───────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// ─────────────── JWT Configs ───────────────
builder.Services.Configure<JwtConfig>(
    builder.Configuration.GetSection(JwtConfig.SectionName));
builder.Services.Configure<LockoutConfig>(
    builder.Configuration.GetSection(LockoutConfig.SectionName));
builder.Services.Configure<SmsConfig>(
    builder.Configuration.GetSection(SmsConfig.SectionName));
builder.Services.Configure<RequestLoggingConfig>(
    builder.Configuration.GetSection(RequestLoggingConfig.SectionName));

// ─────────────── Services and Repositories ───────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IFileManager, FileManager>();
builder.Services.AddSingleton<ISmsService, SmsService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<PermissionSeeder>();
builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddHostedService<PermissionSeedHostedService>();
}


// JWT Config
var jwtSection = builder.Configuration.GetSection("JWT");
var jwtConfig = jwtSection.Get<JwtConfig>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ─────────────── Controllers ───────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ─────────────── Environment-specific DB setup ───────────────
if (!builder.Environment.IsEnvironment("Test")) // این شرط بسیار مهم است
{
    var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("ServerDbConnection")
    : builder.Configuration.GetConnectionString("ProductionDbConnection");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString).EnableDetailedErrors());

    // Hangfire (هم به SQL نیاز داره)
    builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
    builder.Services.AddHangfireServer();
}

// ─────────────── Swagger ───────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "305 .Net",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token..."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// _______________________________ Cors __________________________________
if (!builder.Environment.IsDevelopment())
{
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("FinalPolicy", policy =>
		{
			policy
				.WithOrigins(
					"https://your-domain.com"
				)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials(); // ⬅️ برای ارسال و دریافت کوکی
		});
	});
}
else
{
	builder.Services.AddCors(options =>
	{
		options.AddDefaultPolicy(policy =>
		{
			policy
				.SetIsOriginAllowed(_ => true)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
	});
}
// ─────────────── JWT Auth ───────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = jwtConfig.ValidateIssuer,
            ValidateAudience = jwtConfig.ValidateAudience,
            ValidateLifetime = jwtConfig.ValidateLifetime,
            ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SigningKey)),
            NameClaimType = ClaimTypes.NameIdentifier // این خط را اضافه کنید تا `NameIdentifier` به درستی ست شود
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                    token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                    context.Token = token;

                return Task.CompletedTask;
            }
        };
    });

//─────────────── Rate Limiting ───────────────
builder.Services.AddRateLimiter(options =>
{
	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext,
		string>(httpContext =>
		RateLimitPartition.GetFixedWindowLimiter(
			partitionKey: httpContext.User.Identity?.Name ??
			              httpContext.Request.Headers.Host.ToString(),
	factory: _ => new FixedWindowRateLimiterOptions
	{
		PermitLimit = 120,
		Window = TimeSpan.FromMinutes(1),
		QueueLimit = 0
	}));
});

// ______________________ OutPut Caching ______________________
builder.Services.AddOutputCache();

// ____________________Response Compression____________________
//builder.Services.AddResponseCompression(); // ( Default Gzip/Brotli)
builder.Services.AddResponseCompression(options =>
{
	options.EnableForHttps = true; // یاهتساوخرد یارب یتح یزاسلاعف HTTPS (طایتحا اب)
	options.Providers.Add<BrotliCompressionProvider>();
	options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(opts => opts.Level
	= CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(opts => opts.Level =
	CompressionLevel.SmallestSize);

// ─────────────── SignalR, CORS ───────────────
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCategoryCommand>());

var app = builder.Build();

// ________________ OutPut Caching Middleware ________________
app.UseOutputCache(); // usage sample : on top of controller add this => [OutputCache(Duration = 60)]

// ─────────────── Middlewares ───────────────
if (!app.Environment.IsDevelopment())
{
	app.UseCors("FinalPolicy");
}
else
{
	app.UseCors();
}
app.UseResponseCompression();
if (!app.Environment.IsDevelopment())
{
	app.UseMiddleware<ApiKeyMiddleware>();
	app.UseRateLimiter();
	app.UseHttpsRedirection();
	// Global Error Handler
	app.UseMiddleware<ExceptionHandlingMiddleware>();
}
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "305 API V1");
    });
}

if (!app.Environment.IsEnvironment("Test"))
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = [new HangfireAuthorizationFilter("Admin", "MainAdmin")]
    });
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "logs")),
    RequestPath = "/logs"
});

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

