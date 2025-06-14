using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class UserRoleDataProvider
{
    public static CreateUserRoleCommand Create(string name = "UserRole-name",
        string slug = "UserRole-slug", long userId = 1, long roleId = 1)
        => new()
        {
            name = name,
            slug = slug,
            userid = userId,
            roleid = roleId
        };

    public static EditUserRoleCommand Edit(string name = "name", long id = 1,
        string slug = "slug", long userId = 1, long roleId = 1)
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
            userid = userId,
            roleid = roleId
        };


    public static UserRole Row(string name = "name", long id = 1, string slug = "slug", long userId = 1, long roleId = 1)
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
            userid = userId,
            roleid = roleId
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


    public static GetPaginatedUserRoleQuery GetByQueryFilter(string searchTerm = "")
        => new()
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = searchTerm,
        };

    public static PaginatedList<UserRole> GetPaginatedList()
        => PaginatedListFactory.Create(new List<UserRole>
        {
            new () { id = 1, name = "Tech" },
            new () { id = 2, name = "Health" }
        });
}
