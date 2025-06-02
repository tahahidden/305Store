using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
	private readonly IQueryable<Permission> _queryable;


	public PermissionRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<Permission>();
	}
}
