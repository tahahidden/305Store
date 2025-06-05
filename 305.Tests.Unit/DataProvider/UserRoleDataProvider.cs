using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Features.UserRoleFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public class UserRoleDataProvider
{
	public static CreateUserRoleCommand Create(string name = "UserRole-name", string slug = "UserRole-slug")
		=> new()
		{
			name = name,
			slug = slug,
			userid = 1,
			roleid = 1
		};

	public static EditUserRoleCommand Edit(string name = "name", long id = 1, string slug = "slug")
		=> new()
		{
			id = id,
			name = name,
			slug = slug,
			updated_at = DateTime.Now,
			userid = 1,
			roleid = 1
		};


	public static UserRole Row(string name = "name", long id = 1, string slug = "slug")
		=> new()
		{
			id = id,
			name = name,
			slug = slug,
			updated_at = DateTime.Now,
			userid = 1,
			roleid = 1
		};

	public static DeleteUserRoleCommand Delete(long id = 1)
		=> new()
		{
			id = id,
		};

	public static GetUserRoleBySlugQuery GetBySlug(string slug = "slug")
		=> new()
		{
			slug = slug,
		};

	public static UserRoleResponse GetOne(string slug = "slug", string name = "name")
		=> new()
		{
			id = 1,
			name = name,
			slug = slug,
			userid = 1,
			roleid = 1
		};

	public static GetPaginatedUserRoleQuery GetByQueryFilter(string searchTerm = "")
		=> new()
		{
			Page = 1,
			PageSize = 10,
			SearchTerm = searchTerm,
		};

	public static PaginatedList<UserRole> GetPaginatedList()
		=> new(new List<UserRole>
			{
				new () { id = 1, name = "Tech" },
				new () { id = 2, name = "Health" }
			}
			, count: 2, page: 1, pageSize: 10);
}
