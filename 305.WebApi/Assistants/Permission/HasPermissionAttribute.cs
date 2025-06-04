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
public class HasPermissionFilter : IAsyncAuthorizationFilter
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly string _permission;

	public HasPermissionFilter(string permission, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
	{
		_unitOfWork = unitOfWork;
		_httpContextAccessor = httpContextAccessor;
		_permission = permission;
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var userIdStr = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userIdStr == null || !int.TryParse(userIdStr, out var userId))
		{
			context.Result = new ForbidResult();
			return;
		}

		if (!await _unitOfWork.UserRoleRepository.HasPermissionAsync(userId, _permission))
		{
			context.Result = new ForbidResult();
		}
	}
}

