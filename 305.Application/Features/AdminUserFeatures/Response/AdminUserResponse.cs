using _305.Application.Base.Response;

namespace _305.Application.Features.AdminUserFeatures.Response;

public class AdminUserResponse : BaseResponse
{
	public string email { get; set; }
	public DateTime? last_login_date_time { get; set; }
	public bool is_active { get; set; }
	public bool is_delete_able { get; set; } = true;
}
