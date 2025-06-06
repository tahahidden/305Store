using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _305.Infrastructure.Repository;
public class TokenBlacklistRepository : Repository<BlacklistedToken>, ITokenBlacklistRepository
{
    private readonly IQueryable<BlacklistedToken> _queryable;


    public TokenBlacklistRepository(ApplicationDbContext context) : base(context)
    {
        _queryable = DbContext.Set<BlacklistedToken>();
    }

    public async Task<bool> IsTokenBlacklisted(string token)
    {
        try
        {
            return await _queryable.AnyAsync(x => x.token == token && x.expiry_date > DateTime.UtcNow);
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<BlacklistedToken>?> GetExpiredTokensAsync()
    {

        try
        {
            return await _queryable.Where(t => t.expiry_date <= DateTime.UtcNow).ToListAsync();

        }
        catch
        {
            return null;
        }
    }
}