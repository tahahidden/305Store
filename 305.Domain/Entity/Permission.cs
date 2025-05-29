using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Permission : BaseEntity
{
	public Permission()
	{
		role_permissions = new List<RolePermission>();
	}
	public ICollection<RolePermission> role_permissions { get; set; }
}