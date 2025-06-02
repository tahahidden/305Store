using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using _305.Application.IRepository;

namespace _305.Infrastructure.Repository;
public class UserRepository : Repository<User>, IUserRepository
{
	private readonly IQueryable<User> _queryable;

	public UserRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<User>();
	}
}
