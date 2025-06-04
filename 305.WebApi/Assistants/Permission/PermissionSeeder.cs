using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;

namespace _305.WebApi.Assistants.Permission;

public class PermissionSeeder(IUnitOfWork unitOfWork)
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task SyncPermissionsAsync()
	{
		var scanner = new PermissionScanner();
		var permissions = scanner.ScanPermissions();

		var existingNames = (_unitOfWork.PermissionRepository.FindList()).Select(p => p.name).ToHashSet();

		foreach (var p in permissions)
		{
			if (!existingNames.Contains(p.Permissionname))
			{
				await _unitOfWork.PermissionRepository.AddAsync(new Domain.Entity.Permission
				{
					name = p.Permissionname,
					slug = p.Permissionname,
					created_at = DateTime.Now,
				});
			}
		}

		await _unitOfWork.CommitAsync(CancellationToken.None);
		var mainAdminRole = await _unitOfWork.RoleRepository.FindSingle(x => x.name == "MainAdmin");
		var allPermissions = _unitOfWork.PermissionRepository.FindList();
		var allPermissionRoles = (_unitOfWork.RolePermissionRepository.FindList()).Select(pr => pr.slug).ToHashSet();
		foreach (var permission in allPermissions)
		{
			var slug = SlugHelper.GenerateSlug(permission.name + "AdminRole");
			if (!allPermissionRoles.Contains(slug))
			{
				if (mainAdminRole != null)
					await _unitOfWork.RolePermissionRepository.AddAsync(new RolePermission()
					{
						created_at = DateTime.Now,
						permission_id = permission.id,
						role_id = mainAdminRole.id,
						updated_at = DateTime.Now,
						slug = slug
					});
			}

		}
		await _unitOfWork.CommitAsync(CancellationToken.None);
	}
}
