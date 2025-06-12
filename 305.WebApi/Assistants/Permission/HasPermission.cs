using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// فیلتر مجوز برای بررسی اینکه آیا کاربر دارای مجوز مشخصی هست یا نه.
/// </summary>
/// <param name="permission">نام مجوز مورد نیاز</param>
/// <param name="httpContextAccessor">برای دسترسی به اطلاعات کاربر</param>
/// <param name="permissionChecker">سرویس بررسی مجوز</param>
public class HasPermission(
    string permission,
    IHttpContextAccessor httpContextAccessor,
    IPermissionChecker permissionChecker
) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // استخراج شناسه کاربر از Claims
        var userIdStr = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId))
        {
            // اگر کاربر لاگین نکرده یا Claim نامعتبر باشد
            context.Result = new ForbidResult();
            return;
        }

        // بررسی اینکه آیا کاربر مجوز مشخص‌شده را دارد یا خیر
        var hasPermission = await permissionChecker.HasPermissionAsync(userId, permission);
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}