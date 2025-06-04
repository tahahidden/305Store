using _305.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace _305.Tests.Integration.Base;
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Test"); // مهم

		builder.ConfigureServices(services =>
		{
			// حذف DbContextهای ثبت‌شده
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

			if (descriptor != null)
				services.Remove(descriptor);

			// افزودن InMemory DB
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseInMemoryDatabase("TestDb");
			});

			// ساخت provider جدید و Seed (اختیاری)
			var sp = services.BuildServiceProvider();
			using var scope = sp.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();
		});
	}
}


