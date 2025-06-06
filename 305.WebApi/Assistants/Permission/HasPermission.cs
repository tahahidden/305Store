using _305.Application.IUOW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace _305.WebApi.Assistants.Permission;

public class HasPermission(
	string permission,
	IHttpContextAccessor httpContextAccessor,
	IPermissionChecker permissionChecker
) : IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var userIdStr = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
		if (!int.TryParse(userIdStr, out var userId))
		{
			context.Result = new ForbidResult();
			return;
		}

		if (!await permissionChecker.HasPermissionAsync(userId, permission))
		{
			context.Result = new ForbidResult();
		}
	}
}
