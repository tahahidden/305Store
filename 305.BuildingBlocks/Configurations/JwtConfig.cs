namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// تنظیمات کامل مربوط به توکن‌های JWT شامل تولید و اعتبارسنجی
/// </summary>
public class JwtConfig
{
    public const string SectionName = "Jwt";

    // ───── تنظیمات اعتبارسنجی (برای JwtBearer) ─────
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateAudience { get; set; } = true;
    public bool ValidateIssuer { get; set; } = true;

    // ───── اطلاعات صادرکننده و گیرنده ─────
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    // ───── کلیدهای امنیتی ─────
    public string SigningKey { get; set; } = string.Empty;
    public string AccessTokenSecretKey { get; set; } = string.Empty;
    public string RefreshTokenSecretKey { get; set; } = string.Empty;

    // ───── مدت‌زمان‌های انقضا ─────
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(15);
    public TimeSpan AdminRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(1);
}