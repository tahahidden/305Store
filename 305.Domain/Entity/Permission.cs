using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Permission : BaseEntity
{
    public ICollection<RolePermission>? role_permissions { get; set; }
}