using _305.Application.IUOW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace _305.WebApi.Assistants.Permission;

public class HasPermissionAttribute : TypeFilterAttribute
{
	public HasPermissionAttribute(string permission) : base(typeof(HasPermissionFilter))
	{
		Arguments = [permission];
	}
}

// فیلتر مربوط به بررسی مجوز دسترسی
public class HasPermissionFilter(string permission, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
	: IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var userIdStr = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userIdStr == null || !int.TryParse(userIdStr, out var userId))
		{
			context.Result = new ForbidResult();
			return;
		}

		if (!await unitOfWork.UserRoleRepository.HasPermissionAsync(userId, permission))
		{
			context.Result = new ForbidResult();
		}
	}
}

