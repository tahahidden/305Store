using _305.Application.IUOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// اینترفیس بررسی مجوز کاربر برای دسترسی به قابلیت‌ها.
/// </summary>
public interface IPermissionChecker
{
	/// <summary>
	/// بررسی می‌کند آیا کاربر دارای مجوز مشخصی هست یا نه.
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="permission">نام مجوز</param>
	Task<bool> HasPermissionAsync(int userId, string permission);
}

/// <summary>
/// پیاده‌سازی بررسی مجوز با استفاده از کش.
/// </summary>
public class PermissionChecker(
	IUnitOfWork unitOfWork,
	IMemoryCache cache) : IPermissionChecker
{
	public async Task<bool> HasPermissionAsync(int userId, string permission)
	{
		var cacheKey = $"Permissions_{userId}";

		// اگر مجوزها در کش نبود، از دیتابیس بخون
		if (!cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
		{
			permissions = unitOfWork.UserRoleRepository.FindList(
					predicate: x => x.userid == userId,
					includeFunc: q => q.Include(ur => ur.role)
						.ThenInclude(r => r.role_permissions)
						.ThenInclude(rp => rp.permission))
				.SelectMany(x => x.role.role_permissions.Select(p => p.permission.name))
				.ToHashSet();

			cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10));
		}

		return permissions.Contains(permission);
	}
}
