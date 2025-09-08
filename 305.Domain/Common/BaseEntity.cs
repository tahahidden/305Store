// حذف وابستگی به EF Core برای رعایت اصل SRP
using System.Collections.Generic;

namespace _305.Domain.Common;

/// <summary>
/// کلاس پایه‌ای برای تمام موجودیت‌های (Entity) دیتابیس
/// این کلاس شامل ویژگی‌های مشترک مانند شناسه، زمان ایجاد، زمان به‌روزرسانی، نام و اسلاگ است
/// </summary>
public class BaseEntity : IBaseEntity
{
    /// <summary>
    /// شناسه یکتای رکورد (Primary Key)
    /// </summary>
    public long id { get; set; }

    /// <summary>
    /// زمان ایجاد رکورد 
    /// </summary>
    public DateTime created_at { get; set; }

    /// <summary>
    /// زمان آخرین به‌روزرسانی رکورد 
    /// </summary>
    public DateTime updated_at { get; set; }

    /// <summary>
    /// نام اختیاری موجودیت
    /// </summary>
    public string name { get; set; } = null!;

    /// <summary>
    /// اسلاگ (slug) یکتا برای موجودیت، معمولاً برای URLها یا لینک‌سازی استفاده می‌شود
    /// </summary>
    public string slug { get; set; } = null!;

    /// <summary>
    /// رویدادهای دامنه مرتبط با این موجودیت
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// سازنده پیش‌فرض که تاریخ‌های ایجاد و به‌روزرسانی را تنظیم می‌کند
    /// </summary>
    public BaseEntity()
    {
        created_at = DateTime.Now;
        updated_at = DateTime.Now;
    }

    /// <summary>
    /// سازنده‌ای که مقدار اولیه نام و اسلاگ را دریافت می‌کند
    /// </summary>
    /// <param name="name">نام موجودیت</param>
    /// <param name="slug">اسلاگ یکتا</param>
    public BaseEntity(string name, string slug) : this()
    {
        this.name = name;
        this.slug = slug;
    }

    /// <summary>
    /// افزودن رویداد دامنه به لیست رویدادها
    /// </summary>
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
