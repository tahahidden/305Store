using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Infrastructure.BaseRepository;
using _305.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _305.Infrastructure.Repository;
public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
	private readonly IQueryable<UserRole> _queryable;


	public UserRoleRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<UserRole>();
	}

	public async Task<bool> HasPermissionAsync(long userId, string permissionName)
	{
		return await _queryable
			.Where(ur => ur.userid == userId)
			.SelectMany(ur => ur.role.role_permissions)
			.AnyAsync(rp => rp.permission.name == permissionName);
	}
}
