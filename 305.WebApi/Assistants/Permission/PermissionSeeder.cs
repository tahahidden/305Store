using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using Serilog;

namespace _305.WebApi.Assistants.Permission;

public class PermissionSeeder(IUnitOfWork unitOfWork)
{
	public async Task SyncPermissionsAsync()
	{
		var scanner = new PermissionScanner();
		var permissions = scanner.ScanPermissions();

		var existingSlugs = unitOfWork.PermissionRepository
			.FindList()
			.ToList()
			.Select(p => p.slug)
			.ToHashSet();


		foreach (var p in permissions)
		{
			var slug = p.Permissionname; // فرض: slug همون name هست

			if (!existingSlugs.Contains(slug))
			{
				await unitOfWork.PermissionRepository.AddAsync(new Domain.Entity.Permission
				{
					name = p.Permissionname,
					slug = slug,
					created_at = DateTime.Now,
				});
			}
		}

		try
		{
			await unitOfWork.CommitAsync(CancellationToken.None);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "permissions exist in db");
		}

		var mainAdminRole = await unitOfWork.RoleRepository.FindSingle(x => x.name == "MainAdmin");
		var allPermissions = unitOfWork.PermissionRepository.FindList();
		var allPermissionRoleSlugs = unitOfWork.RolePermissionRepository.FindList().Select(pr => pr.slug).ToHashSet();

		foreach (var permission in allPermissions)
		{
			var slug = SlugHelper.GenerateSlug(permission.name + "MainAdmin");
			if (!allPermissionRoleSlugs.Contains(slug))
			{
				if (mainAdminRole != null)
				{
					await unitOfWork.RolePermissionRepository.AddAsync(new RolePermission()
					{
						created_at = DateTime.Now,
						updated_at = DateTime.Now,
						permission_id = permission.id,
						role_id = mainAdminRole.id,
						slug = slug,
						name = slug
					});
				}
			}
		}

		try
		{
			await unitOfWork.CommitAsync(CancellationToken.None);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "permissions exist in db");
		}

	}

}
