using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;
public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
	private readonly IQueryable<UserRole> _queryable;


	public UserRoleRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<UserRole>();
	}
}
