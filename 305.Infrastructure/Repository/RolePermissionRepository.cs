using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;
public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
{
	private readonly IQueryable<RolePermission> _queryable;


	public RolePermissionRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<RolePermission>();
	}
}
