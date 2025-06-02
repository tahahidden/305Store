using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _305.Infrastructure.Persistence;

/// <summary>
/// کلاس کانتکست اصلی دیتابیس که از DbContext ارث‌بری می‌کند
/// این کلاس مرکز اصلی ارتباط با پایگاه داده در EF Core است
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// سازنده کانتکست که با استفاده از DI تنظیمات مورد نیاز را دریافت می‌کند
    /// </summary>
    /// <param name="options">تنظیمات DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>
    /// متد پیکربندی مدل‌ها هنگام ایجاد یا به‌روزرسانی دیتابیس
    /// </summary>
    /// <param name="modelBuilder">ابزاری برای ساخت مدل دیتابیس</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // بارگذاری تمام پیکربندی‌هایی که از IEntityTypeConfiguration<T> پیروی می‌کنند
        // این پیکربندی‌ها در اسمبلی فعلی تعریف شده‌اند (مثل کلاس‌هایی که جدول‌ها را پیکربندی می‌کنند)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // اگر بخواهید داده‌های اولیه (Seed Data) وارد کنید، از این بخش استفاده کنید
        // مثال: اضافه کردن داده‌های اولیه برای جدول شهرها
        // modelBuilder.Entity<City>().HasData(CitySeed.All);

        base.OnModelCreating(modelBuilder);
    }
}
