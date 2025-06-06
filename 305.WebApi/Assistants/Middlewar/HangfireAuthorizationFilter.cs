using Hangfire.Dashboard;

namespace _305.WebApi.Assistants.Middlewar;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string[] _allowedRoles;

    /// <summary>
    /// فیلتر مجوز Hangfire که فقط کاربران احراز هویت‌شده و دارای نقش مجاز را می‌پذیرد.
    /// </summary>
    /// <param name="allowedRoles">نقش‌های مجاز برای دسترسی به داشبورد</param>
    public HangfireAuthorizationFilter(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles ?? [];
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var user = httpContext.User;

        // بررسی احراز هویت
        if (user?.Identity?.IsAuthenticated != true)
            return false;

        // اگر نقش خاصی تنظیم نشده باشد، فقط احراز هویت کافی است
        return _allowedRoles.Length == 0 ||
               // بررسی نقش‌ها
               _allowedRoles.Any(role => user.IsInRole(role));
    }
}