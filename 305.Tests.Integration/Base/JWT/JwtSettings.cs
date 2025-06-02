namespace _305.Tests.Integration.Base.JWT;
public class JwtSettings
{
	public string Key { get; set; } = null!;
	public string Issuer { get; set; } = null!;
	public string Audience { get; set; } = null!;
}