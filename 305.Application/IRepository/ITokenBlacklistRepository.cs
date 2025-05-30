using _305.Application.IBaseRepository;
using _305.Domain.Entity;

namespace _305.Application.IRepository;
public interface ITokenBlacklistRepository : IRepository<BlacklistedToken>
{
	Task<bool> IsTokenBlacklisted(string token); // Check if a token is blacklisted
	Task<List<BlacklistedToken>?> GetExpiredTokensAsync(); // Optional: Clean up expired tokens
}
