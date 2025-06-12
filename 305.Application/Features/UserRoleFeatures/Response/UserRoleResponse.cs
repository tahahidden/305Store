using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.Features.RoleFeatures.Response;

namespace _305.Application.Features.UserRoleFeatures.Response;
public class UserRoleResponse : BaseResponse
{
    public long roleid { get; set; }
    public long userid { get; set; }

    public UserResponse? user { get; set; }
    public RoleResponse? role { get; set; }
}
