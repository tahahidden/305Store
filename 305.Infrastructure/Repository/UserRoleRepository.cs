using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using _305.Application.IRepository;

namespace _305.Infrastructure.Repository;
public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
	private readonly IQueryable<UserRole> _queryable;


	public UserRoleRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<UserRole>();
	}
}
