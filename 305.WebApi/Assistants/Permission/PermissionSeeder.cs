using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;

namespace _305.WebApi.Assistants.Permission;

public class PermissionSeeder(IUnitOfWork unitOfWork)
{
	public async Task SyncPermissionsAsync()
	{
		var scanner = new PermissionScanner();
		var permissions = scanner.ScanPermissions();

		var existingNames = (unitOfWork.PermissionRepository.FindList()).Select(p => p.name).ToHashSet();

		foreach (var p in permissions.Where(p => !existingNames.Contains(p.Permissionname)))
		{
			await unitOfWork.PermissionRepository.AddAsync(new Domain.Entity.Permission
			{
				name = p.Permissionname,
				slug = p.Permissionname,
				created_at = DateTime.Now,
			});
		}

		await unitOfWork.CommitAsync(CancellationToken.None);
		var mainAdminRole = await unitOfWork.RoleRepository.FindSingle(x => x.name == "MainAdmin");
		var allPermissions = unitOfWork.PermissionRepository.FindList();
		var allPermissionRoles = (unitOfWork.RolePermissionRepository.FindList()).Select(pr => pr.slug).ToHashSet();
		foreach (var permission in allPermissions)
		{
			var slug = SlugHelper.GenerateSlug(permission.name + "AdminRole");
			if (allPermissionRoles.Contains(slug)) continue;
			if (mainAdminRole != null)
				await unitOfWork.RolePermissionRepository.AddAsync(new RolePermission()
				{
					created_at = DateTime.Now,
					permission_id = permission.id,
					role_id = mainAdminRole.id,
					updated_at = DateTime.Now,
					slug = slug
				});

		}
		await unitOfWork.CommitAsync(CancellationToken.None);
	}
}
