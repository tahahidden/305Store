using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using _305.Application.IRepository;

namespace _305.Infrastructure.Repository;
public class RoleRepository : Repository<Role>, IRoleRepository
{
	private readonly IQueryable<Role> _queryable;

	public RoleRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<Role>();
	}
}