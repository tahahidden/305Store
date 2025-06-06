using System.Reflection;
using _305.WebApi.Base;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Assistants.Permission;

public class PermissionScanner
{
	public List<(string Controller, string Action, string Permissionname)> ScanPermissions()
{
    var assembly = Assembly.GetExecutingAssembly();
    
    var controllers = assembly.GetTypes()
        .Where(type => typeof(BaseController).IsAssignableFrom(type) && !type.IsAbstract);

    var result = new List<(string, string, string)>();

    foreach (var controller in controllers)
    {
        var controllerName = controller.Name.Replace("Controller", "");
        var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

        foreach (var method in methods)
        {
            var permissionAttr = method.GetCustomAttribute<Permission>();
            var permissionName = permissionAttr?.Name ?? $"{controllerName}.{method.Name}";

            result.Add((controllerName, method.Name, permissionName));
        }
    }

    return result;
}
}
