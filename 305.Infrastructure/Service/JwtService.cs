using _305.Application.IService;
using _305.BuildingBlocks.Configurations;
using _305.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace _305.Infrastructure.Service;
public class JwtService(IOptions<JwtConfig> options) : IJwtService
{
	private readonly JwtConfig _config = options.Value;

	public string GenerateAccessToken(User user, List<string?> roles, IEnumerable<Claim>? extraClaims = null)
		=> GenerateToken(user, roles!, _config.AccessTokenSecretKey, _config.AccessTokenLifetime, extraClaims);

	public string GenerateRefreshToken()
	{
		var randomBytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomBytes);
		return Convert.ToBase64String(randomBytes);
	}

	private string GenerateToken(User user, List<string> roles, string key, TimeSpan lifetime, IEnumerable<Claim>? extraClaims)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
			new Claim(ClaimTypes.Name, user.name),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		claims.AddRange(roles.Where(r => r != null).Select(role => new Claim(ClaimTypes.Role, role!)));
		if (extraClaims != null)
			claims.AddRange(extraClaims);

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _config.Issuer,
			audience: _config.Audience,
			claims: claims,
			expires: DateTime.UtcNow.Add(lifetime),
			signingCredentials: credentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public bool ValidateToken(string token)
	{
		try
		{
			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(token);
			if (jwt == null || jwt.ValidTo < DateTime.UtcNow) return false;
			if (jwt.Issuer != _config.Issuer || !jwt.Audiences.Contains(_config.Audience)) return false;
			return true;
		}
		catch { return false; }
	}

	public JwtPayload? GetPayload(string token)
	{
		if (!ValidateToken(token)) return null;
		var handler = new JwtSecurityTokenHandler();
		var jwt = handler.ReadJwtToken(token);
		return jwt.Payload;
	}

	public string GetUsername(string token)
	{
		var payload = GetPayload(token);
		return payload?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
	}

	public string? GetUserIdFromClaims(ClaimsPrincipal user)
		=> user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false,
			ValidateIssuer = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AccessTokenSecretKey)),
			ValidateLifetime = false
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

		if (securityToken is not JwtSecurityToken jwtToken ||
			!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
			throw new SecurityTokenException("Invalid token");

		return principal;
	}

	public int GetTokenExpiryMinutes(string token)
	{
		var handler = new JwtSecurityTokenHandler();
		if (handler.ReadToken(token) is not JwtSecurityToken jwt || jwt.Payload.Exp == null)
			return 0;
		var expDateTime = DateTimeOffset.FromUnixTimeSeconds(jwt.Payload.Exp.Value).UtcDateTime;
		return Math.Max((int)(expDateTime - DateTime.UtcNow).TotalMinutes, 0);
	}
}
