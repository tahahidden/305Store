using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace _305.Tests.Integration.Base.JWT;

public class JwtTestHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTestHelper()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        _jwtSettings = config.GetSection("Jwt").Get<JwtSettings>()!;
    }

    public string GenerateToken(
        string userId = "1",
        string userName = "test-user",
        IEnumerable<string>? roles = null,
        IDictionary<string, string>? extraClaims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (roles != null)
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        if (extraClaims != null)
            claims.AddRange(extraClaims.Select(kvp => new Claim(kvp.Key, kvp.Value)));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void AddTokenToClient(HttpClient client,
        string? userId = null,
        string? userName = null,
        IEnumerable<string>? roles = null,
        IDictionary<string, string>? extraClaims = null)
    {
        var token = GenerateToken(userId ?? "1", userName ?? "test-user", roles, extraClaims);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
