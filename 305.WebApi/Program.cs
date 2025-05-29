using _305.BuildingBlocks.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// مقداردهی تنظیمات JWT از بخش "Jwt" در appsettings.json
// و ثبت آن در سیستم تزریق وابستگی به‌صورت options pattern (IOptions<JwtSetting>)
builder.Services.Configure<JwtConfig>(
	builder.Configuration.GetSection(JwtConfig.SectionName));

// خواندن تنظیمات قفل شدن حساب (Lockout) از appsettings و ثبت آن به‌عنوان یک سرویس قابل تزریق
builder.Services.Configure<LockoutConfig>(
	builder.Configuration.GetSection(LockoutConfig.SectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
