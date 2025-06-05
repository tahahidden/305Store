using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Query;
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
using System.Linq;

namespace _305.Tests.Integration.Base.Factory;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // کانکشن SQLite باید در فیلد کلاس ذخیره شود تا تا پایان تست زنده بماند
    private SqliteConnection _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // تعیین محیط اجرای تست (مهم برای استفاده از تنظیمات مخصوص تست)
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // حذف ثبت قبلی DbContext (در صورت وجود)
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // ساخت و باز کردن کانکشن SQLite در حافظه
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // ثبت DbContext با استفاده از کانکشن SQLite زنده
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_connection);
                options.EnableSensitiveDataLogging(); // برای اشکال‌زدایی بهتر
            });

            // ثبت سایر سرویس‌های مورد نیاز
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCategoryCommand>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EditCategoryCommand>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DeleteCategoryCommand>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllCategoryQuery>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetCategoryBySlugQuery>());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetPaginatedCategoryQuery>());
            // ساخت سرویس‌پروایدر موقت برای ایجاد scope و اجرای دستور ایجاد دیتابیس
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // ایجاد جداول دیتابیس (یا اجرای Migration اگر داری)
            db.Database.EnsureCreated();
            // db.Database.Migrate(); // اگر از Migration استفاده می‌کنی می‌تونی این خط رو فعال کنی
        });
    }

    // بستن و آزادسازی کانکشن SQLite در پایان تست
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }
}
