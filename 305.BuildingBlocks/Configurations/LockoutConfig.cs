namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// کلاس پیکربندی قفل شدن حساب کاربری بعد از تلاش‌های ناموفق ورود
/// از این کلاس معمولاً برای خواندن تنظیمات از appsettings استفاده می‌شود
/// </summary>
public class LockoutConfig
{
    /// <summary>
    /// نام بخشی که تنظیمات Lockout در appsettings.json باید داخل آن تعریف شود
    /// </summary>
    public const string SectionName = "Lockout";

    /// <summary>
    /// حداکثر تعداد تلاش ناموفق ورود قبل از قفل شدن حساب
    /// </summary>
    public int FailedLoginLimit { get; set; } = 4;

    /// <summary>
    /// مدت‌زمان قفل شدن حساب پس از رسیدن به حد مجاز تلاش‌های ناموفق
    /// </summary>
    public TimeSpan LockoutDuration { get; set; } = TimeSpan.FromMinutes(1);
}
