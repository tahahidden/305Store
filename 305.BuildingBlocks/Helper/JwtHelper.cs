using _305.BuildingBlocks.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _305.BuildingBlocks.Helper;

public static class JwtHelper
{
    // بارگذاری تنظیمات توکن امنیتی
    private static readonly JwtConfig Config = new();

    // ساخت توکن دسترسی (Access Token) با استفاده از تنظیمات مربوطه
    public static string CreateJwtAccessToken(long userId, string username) =>
        CreateJwt(userId, username, Config.AccessTokenSecretKey, Config.AccessTokenLifetime);

    // ساخت توکن تازه‌سازی (Refresh Token) با استفاده از تنظیمات مربوطه
    public static string CreateJwtRefreshToken(long userId, string username) =>
        CreateJwt(userId, username, Config.RefreshTokenSecretKey, Config.RefreshTokenLifetime);

    // متد داخلی برای ساخت توکن JWT با مشخصات داده شده
    private static string CreateJwt(long userId, string username, string key, TimeSpan lifetime, IEnumerable<Claim>? extraClaims = null)
    {
        // ایجاد کلید امنیتی متقارن از رشته کلید
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        // تعریف شیوه امضا با الگوریتم HMAC SHA256
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // تعریف لیست ادعاها (Claims) شامل شناسه کاربر، نام کاربری و شناسه یکتا (Jti)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username.ToLowerInvariant()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // افزودن ادعاهای اضافی در صورت وجود
        if (extraClaims != null)
            claims.AddRange(extraClaims);

        // ایجاد شیء توکن JWT با مشخصات شامل صادرکننده، گیرنده، ادعاها، زمان انقضا و امضا
        var token = new JwtSecurityToken(
            issuer: Config.Issuer,
            audience: Config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(lifetime),
            signingCredentials: credentials
        );

        // تبدیل توکن به رشته و بازگرداندن آن
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // اعتبارسنجی یک رشته توکن JWT
    public static bool Validate(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken == null)
                return false;

            var payload = jwtToken.Payload;

            // بررسی صادرکننده توکن با مقدار تنظیم شده
            if (payload.Iss != Config.Issuer)
                return false;

            // بررسی گیرنده (Audience) توکن
            if (!payload.Aud.Contains(Config.Audience))
                return false;

            // بررسی منقضی شدن توکن نسبت به زمان جاری
            if (payload.ValidTo < DateTime.UtcNow)
                return false;

            // اگر همه موارد صحیح بود، توکن معتبر است
            return true;
        }
        catch
        {
            // در صورت بروز خطا در خواندن یا اعتبارسنجی، توکن نامعتبر است
            return false;
        }
    }

    // دریافت payload توکن در صورت اعتبارسنجی صحیح
    public static JwtPayload? GetPayload(string token)
    {
        if (string.IsNullOrEmpty(token) || !Validate(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Payload;
    }

    // دریافت نام کاربری از ادعاهای توکن
    public static string GetUsername(string token)
    {
        var payload = GetPayload(token);
        return payload?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
}
