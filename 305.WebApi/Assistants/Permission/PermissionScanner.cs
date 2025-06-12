using _305.WebApi.Base;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// اسکنر مجوزها برای یافتن متدهایی که دارای Attribute مجوز هستند یا به صورت پیش‌فرض باید مجوزی دریافت کنند.
/// </summary>
public class PermissionScanner
{
    /// <summary>
    /// اسکن تمام کنترلرهای مشتق‌شده از BaseController و جمع‌آوری مجوزهای تعریف‌شده یا پیش‌فرض.
    /// </summary>
    /// <returns>
    /// لیستی از نام کنترلر، نام اکشن و نام مجوز
    /// </returns>
    public List<(string Controller, string Action, string PermissionName)> ScanPermissions()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // گرفتن همه کنترلرهایی که از BaseController ارث برده‌اند (و انتزاعی نیستند)
        var controllers = assembly.GetTypes()
            .Where(type => typeof(BaseController).IsAssignableFrom(type) && !type.IsAbstract);

        var result = new List<(string Controller, string Action, string PermissionName)>();

        foreach (var controller in controllers)
        {
            var controllerName = controller.Name.Replace("Controller", "");

            // فقط متدهای عمومی تعریف‌شده در خود کنترلر (نه ارث‌بری) که NonAction نیستند
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

            foreach (var method in methods)
            {
                // گرفتن مقدار مشخص‌شده در Attribute (در صورت وجود)
                var permissionAttr = method.GetCustomAttribute<PermissionAttribute>();
                var permissionName = permissionAttr?.Name ?? $"{controllerName}.{method.Name}";

                result.Add((controllerName, method.Name, permissionName));
            }
        }

        return result;
    }
}