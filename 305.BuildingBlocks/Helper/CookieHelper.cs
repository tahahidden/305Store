using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار کمکی برای مدیریت کوکی‌های مربوط به JWT.
/// </summary>
public static class CookieHelper
{
    /// <summary>
    /// نام کوکی JWT
    /// </summary>
    private const string JwtCookieName = "jwt";

    /// <summary>
    /// افزودن کوکی رفرش توکن با تنظیمات پیش‌فرض.
    /// </summary>
    /// <param name="response">پاسخ HTTP برای افزودن کوکی</param>
    /// <param name="token">رفرش توکن</param>
    /// <param name="lifetime">مدت زمان اعتبار کوکی</param>
    /// <param name="secure">استفاده از HTTPS</param>
    public static void SetJwtCookie(HttpResponse response, string token, TimeSpan lifetime, bool secure = false)
    {
        response.Cookies.Append(JwtCookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.Add(lifetime)
        });
    }

    /// <summary>
    /// خواندن مقدار کوکی JWT.
    /// </summary>
    public static string? ReadJwtCookie(HttpRequest request)
    {
        return request.Cookies.TryGetValue(JwtCookieName, out var token) ? token : null;
    }

    /// <summary>
    /// حذف کوکی JWT.
    /// </summary>
    public static void DeleteJwtCookie(HttpResponse response)
    {
        response.Cookies.Delete(JwtCookieName);
    }
}
