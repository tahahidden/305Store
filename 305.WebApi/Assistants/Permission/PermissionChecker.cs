using _305.Application.IUOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace _305.WebApi.Assistants.Permission;

public interface IPermissionChecker
{
	Task<bool> HasPermissionAsync(int userId, string permission);
}

public class PermissionChecker(
	IUnitOfWork unitOfWork,
	IMemoryCache cache) : IPermissionChecker
{
	public async Task<bool> HasPermissionAsync(int userId, string permission)
	{
		var cacheKey = $"Permissions_{userId}";
		if (!cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
		{
			permissions = unitOfWork.UserRoleRepository.FindList(
					predicate: x => x.userid == userId,
					includeFunc: x => x.Include(y => y.role)
						.ThenInclude(role => role.role_permissions)
						.ThenInclude(rolePermission => rolePermission.permission))
				.Select(x => x.role.role_permissions.Select(p => p.permission.name))
				.SelectMany(x => x)
				.ToHashSet();

			cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10));
		}

		return permissions.Contains(permission);
	}
}
