using System.ComponentModel.DataAnnotations;

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
	[MaxLength(1000)]
	public string slug { get; set; } = null!;

	/// <summary>
	/// سازنده پیش‌فرض که تاریخ‌های ایجاد و به‌روزرسانی را تنظیم می‌کند
	/// </summary>
        public BaseEntity()
        {
                created_at = DateTime.UtcNow;
                updated_at = DateTime.UtcNow;
        }
}
