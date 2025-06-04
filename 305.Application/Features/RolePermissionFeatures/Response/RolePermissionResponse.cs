using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Response;
using _305.Application.Features.RoleFeatures.Response;

namespace _305.Application.Features.RolePermissionFeatures.Response;
public class RolePermissionResponse : BaseResponse
{
	public long role_id { get; set; }
	public RoleResponse? role { get; set; }

	public long permission_id { get; set; }
	public PermissionResponse? permission { get; set; }
}
