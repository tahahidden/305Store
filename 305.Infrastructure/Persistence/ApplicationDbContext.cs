using _305.Domain.Entity;
using _305.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _305.Infrastructure.Persistence
{
	/// <summary>
	/// DbContext اصلی برنامه که مدیریت اتصال به دیتابیس و پیکربندی مدل‌ها را بر عهده دارد.
	/// </summary>
	public class ApplicationDbContext : DbContext
	{
		/// <summary>
		/// سازنده کلاس، تنظیمات DbContext را از DI دریافت می‌کند.
		/// </summary>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		/// <summary>
		/// پیکربندی مدل‌ها در زمان ساخت مدل دیتابیس.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ApplyEntityConfigurations(modelBuilder);
			SeedInitialData(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// اعمال تمام پیکربندی‌های موجود در اسمبلی فعلی که از IEntityTypeConfiguration پیروی می‌کنند.
		/// </summary>
		/// <param name="modelBuilder"></param>
		private static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
			=> modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		/// <summary>
		/// افزودن داده‌های اولیه (Seed Data) به جداول مختلف.
		/// این متد جدا شده برای وضوح بیشتر.
		/// </summary>
		/// <param name="modelBuilder"></param>
		private static void SeedInitialData(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(UserSeed.All);
			modelBuilder.Entity<Role>().HasData(RoleSeed.All);
			modelBuilder.Entity<UserRole>().HasData(UserRoleSeed.All);
		}
	}
}