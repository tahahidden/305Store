namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// کلاس پیکربندی مربوط به توکن‌های امنیتی JWT شامل کلیدها و مدت‌زمان‌های انقضا
/// از این کلاس معمولاً برای خواندن تنظیمات از appsettings.json استفاده می‌شود
/// </summary>
public class SecurityTokenConfig
{
    /// <summary>
    /// نام بخشی که تنظیمات توکن امنیتی در appsettings.json باید داخل آن تعریف شود
    /// </summary>
    public const string SectionName = "SecurityToken";

    /// <summary>
    /// کلید عمومی یا پایه رمزگذاری (در صورت استفاده عمومی)
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// صادرکننده توکن (Issuer) - معمولاً نام یا آدرس سرور صادرکننده توکن
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// دریافت‌کننده توکن (Audience) - معمولاً کلاینتی که توکن برای آن صادر شده است
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// کلید مخفی برای تولید توکن دسترسی (Access Token)
    /// </summary>
    public string AccessTokenSecretKey { get; set; } = string.Empty;

    /// <summary>
    /// مدت زمان اعتبار توکن دسترسی
    /// </summary>
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);

    /// <summary>
    /// کلید مخفی برای تولید توکن رفرش (Refresh Token)
    /// </summary>
    public string RefreshTokenSecretKey { get; set; } = string.Empty;

    /// <summary>
    /// مدت زمان اعتبار توکن رفرش معمولی (کاربر عادی)
    /// </summary>
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(15);

    /// <summary>
    /// مدت زمان اعتبار توکن رفرش برای مدیر (امنیت بالاتر و زمان کوتاه‌تر)
    /// </summary>
    public TimeSpan AdminRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(1);
}
