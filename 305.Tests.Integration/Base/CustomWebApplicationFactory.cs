using _305.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace _305.Tests.Integration.Base;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			// حذف همه‌ی سرویس‌هایی که AppDbContext ثبت کرده
			var dbContextDescriptors = services
				.Where(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))
				.ToList();

			foreach (var descriptor in dbContextDescriptors)
				services.Remove(descriptor);

			// ثبت مجدد با InMemory
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseInMemoryDatabase("TestDb");
			});

			// ساختن Provider جدید
			var sp = services.BuildServiceProvider();

			using var scope = sp.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			db.Database.EnsureDeleted(); // برای اطمینان
			db.Database.EnsureCreated();
		});
	}
}
