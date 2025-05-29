using _305.Domain.Common;

namespace _305.Domain.Entity;
public class RolePermission : BaseEntity
{
	public long role_id { get; set; }
	public Role role { get; set; } = default!;

	public long permission_id { get; set; }
	public Permission permission { get; set; } = default!;
}