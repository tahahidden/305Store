using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class PermissionDataProvider
{
    public static CreatePermissionCommand Create(string name = "Permission-name", string slug = "Permission-slug")
        => new()
        {
            name = name,
            slug = slug
        };

    public static EditPermissionCommand Edit(string name = "name", long id = 1, string slug = "slug")
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
        };


    public static Permission Row(string name = "name", long id = 1, string slug = "slug")
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
        };

    public static DeletePermissionCommand Delete(long id = 1)
        => new()
        {
            id = id,
        };

    public static GetPermissionBySlugQuery GetBySlug(string slug = "slug")
        => new()
        {
            slug = slug,
        };

    public static GetPaginatedPermissionQuery GetByQueryFilter(string searchTerm = "")
        => new()
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = searchTerm,
        };

    public static PaginatedList<Permission> GetPaginatedList()
        => PaginatedListFactory.Create(new List<Permission>
        {
            new () { id = 1, name = "Tech" },
            new () { id = 2, name = "Health" }
        });
}
