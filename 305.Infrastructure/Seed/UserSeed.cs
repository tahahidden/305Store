using _305.Domain.Entity;

namespace _305.Infrastructure.Seed;

public static class UserSeed
{
	public static List<User> All =>
	[
		new()
		{
			id = 1,
			email = "info@305.com",
			failed_login_count = 0,
			is_locked_out = false,
			is_delete_able = false,
			mobile = "09309309393",
			name = "admin-user",
			password_hash = "omTtMfA5EEJCzjH5t/Q67cRXK5TRwerSqN7sJSm41No=.FRLmTm9jwMcEFnjpjgivJw==", // QAZqaz!@#123
			concurrency_stamp = "X3JO2EOCURAEBU6HHY6OBYEDD2877FXU",
			security_stamp = "098NTB7E5LFFXREHBSEHDKLI0DOBIKST",
			created_at = new DateTime(2025, 1, 1, 12, 0, 0),
			updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
			slug= "Admin-User",
			is_active = true,
			is_mobile_confirmed = true,
			last_login_date_time = new DateTime(2025, 1, 1, 12, 0, 0),
			lock_out_end_time = new DateTime(2025, 1, 1, 12, 0, 0),
			refresh_token = "refeshToken",
			refresh_token_expiry_time = new DateTime(2026, 1, 1, 12, 0, 0),
		}
	];
}