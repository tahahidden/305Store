using _305.Domain.Entity;

namespace _305.Infrastructure.Seed;

public static class UserRoleSeed
{
	public static List<UserRole> All =>
	[
		new()
		{
			id = 1,
			name = "Main Admin User",
			role_id = 3,
			user_id = 1,
			created_at =  new DateTime(2025, 1, 1, 12, 0, 0),
			updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
			slug = "Main-Admin-User",

		}
	];
}