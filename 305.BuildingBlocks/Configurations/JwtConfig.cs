namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// تنظیمات کامل مربوط به توکن‌های JWT شامل تولید و اعتبارسنجی
/// </summary>
public class JwtConfig
{
    public const string SectionName = "Jwt";

    // ───── تنظیمات اعتبارسنجی (برای JwtBearer) ─────

    // بررسی اینکه آیا امضای توکن باید اعتبارسنجی شود یا نه (برای اطمینان از اینکه توکن دستکاری نشده)
    public bool ValidateIssuerSigningKey { get; set; } = true;

    // بررسی انقضای توکن (اگر false باشد، توکن حتی اگر منقضی شده باشد پذیرفته می‌شود - ناامن!)
    public bool ValidateLifetime { get; set; } = true;

    // بررسی اینکه آیا مقدار Audience در توکن با مقدار تنظیم‌شده یکی است یا نه (برای محدود کردن مصرف‌کننده‌ها)
    public bool ValidateAudience { get; set; } = true;

    // بررسی اینکه آیا صادرکننده توکن (Issuer) معتبر است یا نه
    public bool ValidateIssuer { get; set; } = true;


    // ───── اطلاعات صادرکننده و گیرنده ─────

    // رشته‌ای که نشان می‌دهد توکن توسط چه سیستمی صادر شده (مثلاً: "myapp.com")
    public string Issuer { get; set; } = string.Empty;

    // رشته‌ای که نشان می‌دهد توکن برای چه سیستمی صادر شده (مثلاً: "myapp-client")
    public string Audience { get; set; } = string.Empty;


    // ───── کلیدهای امنیتی ─────

    // کلید اصلی برای امضای توکن (می‌تواند برای کاربردهای عمومی‌تر استفاده شود)
    public string SigningKey { get; set; } = string.Empty;

    // کلید مخصوص امضای Access Token (می‌تواند متفاوت از SigningKey باشد برای تفکیک بهتر)
    public string AccessTokenSecretKey { get; set; } = string.Empty;

    // کلید مخصوص امضای Refresh Token (برای افزایش امنیت بهتر است با AccessToken متفاوت باشد)
    public string RefreshTokenSecretKey { get; set; } = string.Empty;


    // ───── مدت‌زمان‌های انقضا ─────

    // مدت اعتبار Access Token (مثلاً ۱۵ دقیقه - توکن‌های کوتاه‌مدت برای درخواست‌های معمول)
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);

    // مدت اعتبار Refresh Token (مثلاً ۱۵ روز - برای دریافت توکن جدید بدون نیاز به ورود مجدد)
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(15);

    // مدت اعتبار Refresh Token مخصوص ادمین‌ها (مثلاً ۱ روز - برای محدود کردن ماندگاری توکن‌های حساس‌تر)
    public TimeSpan AdminRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(1);

}