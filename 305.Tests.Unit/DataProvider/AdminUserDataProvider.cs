using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Features.AdminUserFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class AdminUserDataProvider
{
	public static CreateAdminUserCommand Create(string name = "name")
=> new CreateAdminUserCommand()
{
	name = name,
	created_at = DateTime.Now,
	slug = null,
	updated_at = DateTime.Now,
	email = "mamad@304.com",
	password = "password",
};

	public static EditAdminUserCommand Edit(string name = "name", long id = 1)
		=> new EditAdminUserCommand()
		{
			id = id,
			name = name,
			slug = null,
			updated_at = DateTime.Now,
			email = "mamad@304.com",
			password = "password",
		};


	public static User Row(string name = "name", long id = 1, string slug = "slug")
	=> new User()
	{
		id = id,
		email = "info@304.com",
		failed_login_count = 0,
		is_locked_out = false,
		is_delete_able = false,
		mobile = "09309309393",
		name = name,
		password_hash = "omTtMfA5EEJCzjH5t/Q67cRXK5TRwerSqN7sJSm41No=.FRLmTm9jwMcEFnjpjgivJw==", // QAZqaz!@#123
		concurrency_stamp = "X3JO2EOCURAEBU6HHY6OBYEDD2877FXU",
		security_stamp = "098NTB7E5LFFXREHBSEHDKLI0DOBIKST",
		created_at = new DateTime(2025, 1, 1, 12, 0, 0),
		updated_at = new DateTime(2025, 1, 1, 12, 0, 0),
		slug = slug,
		is_active = true,
		is_mobile_confirmed = true,
		last_login_date_time = DateTime.Now,
		lock_out_end_time = DateTime.Now,
		refresh_token = "refresh token",
		refresh_token_expiry_time = DateTime.Now.AddDays(15),
	};

	public static DeleteAdminUserCommand Delete(long id = 1)
		=> new DeleteAdminUserCommand()
		{
			id = id,
		};

	public static GetAdminUserBySlugQuery GetBySlug(string slug = "slug")
	=> new GetAdminUserBySlugQuery()
	{
		slug = slug,
	};

	public static AdminUserResponse GetOne(string slug = "slug", string name = "name")
		=> new AdminUserResponse()
		{
			id = 1,
			name = name,
			slug = slug,
			created_at = DateTime.Now,
			email = "email@304.com",
			is_active = true,
			is_delete_able = true,
			last_login_date_time = DateTime.Now,
			updated_at = DateTime.Now
		};

	public static GetPaginatedAdminUserQuery GetByQueryFilter(string searchTerm = "")
	=> new GetPaginatedAdminUserQuery()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<User> GetPaginatedList()
	=> new PaginatedList<User>(new List<User>
		{
			Row(name: "Tech", id: 1, slug: "tech"),
			Row(name: "Health", id: 2, slug: "health")
		}
	, count: 2, page: 1, pageSize: 10);
}
