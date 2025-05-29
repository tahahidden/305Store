using _305.Domain.Common;

namespace _305.Domain.Entity;
public class UserRole : BaseEntity
{
	public long role_id { get; set; }
	public long user_id { get; set; }

	public User user { get; set; } = default!;
	public Role role { get; set; } = default!;

}