using _305.Domain.Common;

namespace _305.Domain.Entity;
public class RolePermission : BaseEntity
{
    public long role_id { get; set; }
    public Role? role { get; set; }

    public long permission_id { get; set; }
    public Permission? permission { get; set; }

    /// <summary>
    /// سازنده برای ایجاد ارتباط نقش و سطح دسترسی
    /// </summary>
    public RolePermission(long role_id, long permission_id) : base()
    {
        this.role_id = role_id;
        this.permission_id = permission_id;
    }

    public RolePermission() { }
}