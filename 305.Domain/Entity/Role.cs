using _305.Domain.Common;

namespace _305.Domain.Entity;
public class Role : BaseEntity
{
    #region Navigations

    public ICollection<UserRole>? user_roles { get; set; }
    public ICollection<RolePermission>? role_permissions { get; set; }

    #endregion

    /// <summary>
    /// سازنده برای ایجاد نقش با مقادیر اولیه
    /// </summary>
    public Role(string name, string slug) : base(name, slug) { }

    public Role() { }
}