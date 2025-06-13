using _305.Domain.Common;

namespace _305.Domain.Entity;
public class UserRole : BaseEntity
{
    public long roleid { get; set; }
    public long userid { get; set; }

    public User? user { get; set; }
    public Role? role { get; set; }

    /// <summary>
    /// سازنده برای ایجاد ارتباط کاربر و نقش
    /// </summary>
    public UserRole(long userid, long roleid) : base()
    {
        this.userid = userid;
        this.roleid = roleid;
    }

    public UserRole() { }
}