using _305.Domain.Common;

namespace _305.Domain.Entity;
public class RolePermission : BaseEntity
{
	public long role_id { get; set; }
	public Role? role { get; set; }

	public long permission_id { get; set; }
	public Permission? permission { get; set; }
}