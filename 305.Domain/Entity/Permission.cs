using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Permission : BaseEntity
{
    public ICollection<RolePermission>? role_permissions { get; set; }

    /// <summary>
    /// سازنده برای ایجاد سطح دسترسی با مقادیر اولیه
    /// </summary>
    public Permission(string name, string slug) : base(name, slug) { }

    public Permission() { }
}