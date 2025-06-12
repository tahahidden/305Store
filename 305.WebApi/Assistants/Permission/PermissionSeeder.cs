using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using Serilog;

namespace _305.WebApi.Assistants.Permission;

/// <summary>
/// همگام‌سازی مجوزهای تعریف‌شده در کنترلرها با دیتابیس.
/// </summary>
public class PermissionSeeder(IUnitOfWork unitOfWork)
{
	/// <summary>
	/// اسکن مجوزها، ذخیره در دیتابیس در صورت جدید بودن و نسبت دادن تمام مجوزها به نقش اصلی "MainAdmin".
	/// </summary>
	public async Task SyncPermissionsAsync()
	{
		var scanner = new PermissionScanner();
		var permissions = scanner.ScanPermissions();

		// گرفتن مجوزهای موجود از دیتابیس
		var existingSlugs = unitOfWork.PermissionRepository
			.FindList()
			.Select(p => p.slug)
			.ToHashSet();

		// افزودن مجوزهای جدید
		foreach (var p in permissions)
		{
			var slug = p.PermissionName;

			if (!existingSlugs.Contains(slug))
			{
				await unitOfWork.PermissionRepository.AddAsync(new Domain.Entity.Permission
				{
					name = p.PermissionName,
					slug = slug,
                                        created_at = DateTime.UtcNow,
				});
			}
		}

		// ذخیره تغییرات مجوزها
		try
		{
			await unitOfWork.CommitAsync(CancellationToken.None);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "خطا در ذخیره مجوزها");
		}

		// تخصیص تمام مجوزها به نقش MainAdmin
		var mainAdminRole = await unitOfWork.RoleRepository.FindSingle(x => x.name == "MainAdmin");
		if (mainAdminRole is null)
		{
			Log.Warning("نقش MainAdmin یافت نشد.");
			return;
		}

		var allPermissions = unitOfWork.PermissionRepository.FindList();
		var assignedSlugs = unitOfWork.RolePermissionRepository
			.FindList()
			.Select(rp => rp.slug)
			.ToHashSet();

		foreach (var permission in allPermissions)
		{
			var slug = SlugHelper.GenerateSlug(permission.name + "MainAdmin");

			if (!assignedSlugs.Contains(slug))
			{
				await unitOfWork.RolePermissionRepository.AddAsync(new RolePermission
				{
					role_id = mainAdminRole.id,
					permission_id = permission.id,
					slug = slug,
					name = slug,
                                        created_at = DateTime.UtcNow,
                                        updated_at = DateTime.UtcNow,
				});
			}
		}

		// ذخیره تغییرات تخصیص مجوزها
		try
		{
			await unitOfWork.CommitAsync(CancellationToken.None);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "خطا در تخصیص مجوزها به نقش MainAdmin");
		}
	}
}
