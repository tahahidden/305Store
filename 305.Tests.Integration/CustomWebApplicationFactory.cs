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
            // حذف قبلی DbContext (مخصوص EF Core)
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // ثبت DbContext با InMemory برای تست
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open(); // مهم!

                options.UseSqlite(connection);
            });

            // ثبت Repository و UnitOfWork
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ثبت MediatR (اگر لازمه)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCategoryCommand>());
        });
    }
}
