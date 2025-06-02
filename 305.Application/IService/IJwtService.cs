using _305.Domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace _305.Application.IService;
public interface IJwtService
{
	string GenerateAccessToken(User user, List<string> roles, IEnumerable<Claim>? extraClaims = null);
	string GenerateRefreshToken();
	bool ValidateToken(string token);
	JwtPayload? GetPayload(string token);
	string GetUsername(string token);
	string? GetUserIdFromClaims(ClaimsPrincipal user);
	ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
	int GetTokenExpiryMinutes(string token);
}