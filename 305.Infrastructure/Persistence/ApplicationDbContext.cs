using _305.Domain.Entity;
using _305.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _305.Infrastructure.Persistence;

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
		// اعمال تمام پیکربندی‌های موجود در اسمبلی فعلی که از IEntityTypeConfiguration پیروی می‌کنند
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		// اگر Seed Data دارید، اینجا اضافه کنید.
		SeedData(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	/// <summary>
	/// افزودن داده‌های اولیه (Seed Data) به جداول مختلف.
	/// این متد جدا شده برای وضوح بیشتر.
	/// </summary>
	private static void SeedData(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>().HasData(UserSeed.All);
		modelBuilder.Entity<Role>().HasData(RoleSeed.All);
		modelBuilder.Entity<UserRole>().HasData(UserRoleSeed.All);
	}
}