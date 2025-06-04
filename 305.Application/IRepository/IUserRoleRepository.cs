using _305.Application.IBaseRepository;
using _305.Domain.Entity;

namespace _305.Application.IRepository;
public interface IUserRoleRepository : IRepository<UserRole>
{
	Task<bool> HasPermissionAsync(long userId, string permissionName);
}
