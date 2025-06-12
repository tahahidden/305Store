using _305.Application.IService;
using _305.BuildingBlocks.Configurations;
using _305.Domain.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace _305.Infrastructure.Service
{
	/// <summary>
	/// سرویس مدیریت تولید و اعتبارسنجی توکن‌های JWT.
	/// </summary>
	public class JwtService : IJwtService
	{
		private readonly JwtConfig _config;

		/// <summary>
		/// سازنده کلاس، تنظیمات JWT را از DI دریافت می‌کند.
		/// </summary>
		public JwtService(IOptions<JwtConfig> options)
		{
			_config = options.Value;
		}

		/// <summary>
		/// تولید توکن دسترسی (Access Token) با اطلاعات کاربر و نقش‌ها.
		/// </summary>
		public string GenerateAccessToken(User user, List<string?> roles, IEnumerable<Claim>? extraClaims = null)
			=> GenerateToken(user, roles!, _config.AccessTokenSecretKey, _config.AccessTokenLifetime, extraClaims);

		/// <summary>
		/// تولید توکن تازه (Refresh Token) به صورت رشته تصادفی و امن.
		/// </summary>
		public string GenerateRefreshToken()
		{
			var randomBytes = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomBytes);
			return Convert.ToBase64String(randomBytes);
		}

		/// <summary>
		/// تولید توکن JWT با پارامترهای مشخص.
		/// </summary>
		private string GenerateToken(User user, List<string> roles, string key, TimeSpan lifetime, IEnumerable<Claim>? extraClaims)
		{
			var claims = BuildClaims(user, roles, extraClaims);

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _config.Issuer,
				audience: _config.Audience,
				claims: claims,
				expires: DateTime.UtcNow.Add(lifetime),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		/// <summary>
		/// ساخت لیست ادعاها (Claims) بر اساس اطلاعات کاربر، نقش‌ها و ادعای اضافی.
		/// </summary>
		private static List<Claim> BuildClaims(User user, List<string> roles, IEnumerable<Claim>? extraClaims)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
				new Claim(ClaimTypes.Name, user.name),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			claims.AddRange(roles.Where(r => !string.IsNullOrEmpty(r)).Select(role => new Claim(ClaimTypes.Role, role!)));

			if (extraClaims != null)
				claims.AddRange(extraClaims);

			return claims;
		}

		/// <summary>
		/// اعتبارسنجی کلی توکن JWT (تاریخ انقضا، ناشر و مخاطب).
		/// </summary>
		public bool ValidateToken(string token)
		{
			try
			{
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(token);

				if (jwt == null || jwt.ValidTo < DateTime.UtcNow)
					return false;

				if (jwt.Issuer != _config.Issuer || !jwt.Audiences.Contains(_config.Audience))
					return false;

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// دریافت Payload توکن پس از اعتبارسنجی.
		/// </summary>
		public JwtPayload? GetPayload(string token)
		{
			if (!ValidateToken(token)) return null;

			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(token);
			return jwt.Payload;
		}

		/// <summary>
		/// استخراج نام کاربری از توکن JWT.
		/// </summary>
		public string GetUsername(string token)
		{
			var payload = GetPayload(token);
			return payload?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
		}

		/// <summary>
		/// استخراج شناسه کاربری از ClaimsPrincipal.
		/// </summary>
		public string? GetUserIdFromClaims(ClaimsPrincipal user)
			=> user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		/// <summary>
		/// دریافت ClaimsPrincipal از توکن منقضی شده برای عملیات Refresh Token.
		/// </summary>
		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var validationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AccessTokenSecretKey)),
				ValidateLifetime = false
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtToken ||
				!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}

		/// <summary>
		/// محاسبه دقیقه‌های باقی‌مانده تا انقضای توکن.
		/// </summary>
		public int GetTokenExpiryMinutes(string token)
		{
			var handler = new JwtSecurityTokenHandler();

			if (handler.ReadToken(token) is not JwtSecurityToken jwt || jwt.Payload.Exp == null)
				return 0;

			var expDateTime = DateTimeOffset.FromUnixTimeSeconds(jwt.Payload.Exp.Value).UtcDateTime;
			return Math.Max((int)(expDateTime - DateTime.UtcNow).TotalMinutes, 0);
		}
	}
}
