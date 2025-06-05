using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using _305.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace _305.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 🔧 این باید اینجا باشه، نه داخل ConfigureServices
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            builder.UseEnvironment("Test");

            // حذف DbContext قبلی
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // اتصال in-memory با SQLite
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connection);
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCategoryCommand>());

            // اجرای migration یا ساخت دیتابیس (اختیاری)
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated(); // یا db.Database.Migrate(); اگر از Migration استفاده می‌کنی
        });
    }
}
