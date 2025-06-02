using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace _305.Infrastructure.Persistence
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			try
			{
				// رشته اتصال به دیتابیس (کانکشن استرینگ) به صورت دستی تعریف شده است
				const string connectionString = "Server=.;Database=305_db;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";

				// ساخت یک DbContextOptionsBuilder برای تنظیمات DbContext
				var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

				// استفاده از SQL Server با رشته اتصال داده شده
				optionsBuilder.UseSqlServer(connectionString);

				// ایجاد و بازگرداندن نمونه‌ای از ApplicationDbContext با تنظیمات بالا
				return new ApplicationDbContext(optionsBuilder.Options);
			}
			catch (Exception ex)
			{
				// در صورت بروز خطا، پیام خطا در کنسول چاپ می‌شود
				Console.WriteLine("ERROR creating DbContext: " + ex.Message);

				// خطا مجدداً پرتاب می‌شود تا در سطوح بالاتر مدیریت شود
				throw;
			}
		}
	}

}

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.IO;

//namespace _305.Infrastructure.Persistence
//{
//    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//    {
//        public ApplicationDbContext CreateDbContext(string[] args)
//        {
//            try
//            {
//                // مسیر فولدر پروژه ای که فایل appsettings.json اونجا هست (مثلا WebApi)
//                var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../_305.WebApi"));

//                Console.WriteLine("Loading configuration from: " + basePath);

//                var config = new ConfigurationBuilder()
//                    .SetBasePath(basePath)
//                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
//                    .Build();

//                var connectionString = config.GetConnectionString("DefaultConnection");

//                if (string.IsNullOrWhiteSpace(connectionString))
//                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//                optionsBuilder.UseSqlServer(connectionString);

//                return new ApplicationDbContext(optionsBuilder.Options);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("ERROR creating DbContext: " + ex.Message);
//                throw;
//            }
//        }
//    }
//}
