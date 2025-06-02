namespace _305.Application.Features.AdminAuthFeatures.Response;

public class LoginResponse
{
	public required string access_token { get; set; }
	public double expire_in { get; set; }
}
