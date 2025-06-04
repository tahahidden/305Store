using _305.Application.Base.Response;
using _305.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using _305.Application.Features.RolePermissionFeatures.Response;
using _305.Application.Features.UserRoleFeatures.Response;

namespace _305.Application.Features.RoleFeatures.Response;
public class RoleResponse :BaseResponse
{
	public ICollection<UserRoleResponse>? user_roles { get; set; }
	public ICollection<RolePermissionResponse>? role_permissions { get; set; }
}
