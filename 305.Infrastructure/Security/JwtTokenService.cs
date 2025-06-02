//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//public class JwtTokenService
//{
//	// کلید محرمانه JWT برای امضای توکن‌ها
//	private readonly string _jwtSecret;
//	// صادر کننده توکن
//	private readonly string _jwtIssuer;
//	// مخاطب توکن
//	private readonly string _jwtAudience;
//	// مدت زمان اعتبار توکن (بر حسب دقیقه)
//	private readonly int _tokenExpiryMinutes;

//	// سازنده که تنظیمات را از IConfiguration می‌خواند
//	public JwtTokenService(IConfiguration configuration)
//	{
//		_jwtSecret = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
//		_jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
//		_jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
//		_tokenExpiryMinutes = int.TryParse(configuration["Jwt:ExpiryInMinutes"], out var expiry) ? expiry : 15;
//	}

//	// تولید توکن دسترسی JWT با ادعاهای کاربر و نقش‌ها
//	public string GenerateToken(User user, List<string> roles)
//	{
//		var claims = new List<Claim>
//		{
//			new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
//			new Claim(ClaimTypes.Name, user.user_name),
//			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//		};

//		// افزودن نقش‌ها به لیست ادعاها
//		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

//		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
//		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//		// ایجاد شیء JwtSecurityToken با مشخصات داده شده
//		var token = new JwtSecurityToken(
//			issuer: _jwtIssuer,
//			audience: _jwtAudience,
//			claims: claims,
//			expires: DateTime.UtcNow.AddMinutes(_tokenExpiryMinutes),
//			signingCredentials: credentials
//		);

//		// تبدیل توکن به رشته و بازگشت آن
//		return new JwtSecurityTokenHandler().WriteToken(token);
//	}

//	// تولید توکن رفرش تصادفی و امن به صورت Base64
//	public string GenerateRefreshToken()
//	{
//		var randomBytes = new byte[64];
//		using var rng = RandomNumberGenerator.Create();
//		rng.GetBytes(randomBytes);
//		return Convert.ToBase64String(randomBytes);
//	}

//	// استخراج شناسه کاربر از ClaimsPrincipal
//	public string? GetUserIdFromClaims(ClaimsPrincipal user)
//	{
//		return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
//	}

//	// بازیابی ClaimsPrincipal از توکن منقضی شده (بدون اعتبارسنجی زمان)
//	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
//	{
//		var tokenValidationParameters = new TokenValidationParameters
//		{
//			ValidateAudience = false,
//			ValidateIssuer = false,
//			ValidateIssuerSigningKey = true,
//			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
//			ValidateLifetime = false // اعتبارسنجی زمان غیرفعال است چون توکن منقضی شده
//		};

//		var tokenHandler = new JwtSecurityTokenHandler();
//		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

//		// بررسی صحت الگوریتم امضای توکن
//		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
//			!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
//			throw new SecurityTokenException("Invalid token");

//		return principal;
//	}

//	// گرفتن تعداد دقیقه‌های باقی‌مانده از اعتبار توکن
//	public int GetTokenExpiryMinutes(string token)
//	{
//		var handler = new JwtSecurityTokenHandler();
//		var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

//		if (jwtToken == null)
//			throw new ArgumentException("Invalid token");

//		var expUnix = jwtToken.Payload.Exp;
//		if (!expUnix.HasValue)
//			throw new ArgumentException("Token does not contain expiration");

//		var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnix.Value).UtcDateTime;
//		var remainingMinutes = (int)(expDateTime - DateTime.UtcNow).TotalMinutes;

//		// حداقل 0 دقیقه (اگر منفی شد 0 برگردانده می‌شود)
//		return Math.Max(remainingMinutes, 0);
//	}
//}
