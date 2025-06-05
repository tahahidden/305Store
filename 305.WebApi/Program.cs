using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.IService;
using _305.BuildingBlocks.Configurations;
using _305.BuildingBlocks.IService;
using _305.BuildingBlocks.Service;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.Service;
using _305.WebApi.Assistants.Middlewar;
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

// ─────────────── Services and Repositories ───────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<PermissionSeeder>();
builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();


// JWT Config
var jwtSection = builder.Configuration.GetSection("JWT");
var jwtConfig = jwtSection.Get<JwtConfig>();

builder.Services.AddDistributedMemoryCache();
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

// ─────────────── JWT Auth ───────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
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

// ─────────────── Middlewares ───────────────
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

app.UseCors("AllowSpecific");
// app.UseHttpsRedirection(); // فعال‌سازی اگر نیاز بود

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

app.Lifetime.ApplicationStarted.Register(async void () =>
{
	try
	{
		using var scope = app.Services.CreateScope();
		var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
		await seeder.SyncPermissionsAsync();
	}
	catch (Exception e)
	{
		Log.Error(e, "error during seeding permissions");
	}
});


app.Run();
