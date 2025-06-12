using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;

namespace _305.Infrastructure.Repository;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
	public PermissionRepository(ApplicationDbContext context) : base(context)
	{
		DbContext.Set<Permission>();
	}
}
