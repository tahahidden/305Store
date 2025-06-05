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

		return (from controller in controllers
			let controllerName = controller.Name.Replace("Controller", "")
			let methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
				.Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)))
			from method in methods
			let permissionName = $"{controllerName}.{method.Name}"
			select (controllerName, method.Name, permissionName)).ToList();
	}
}
