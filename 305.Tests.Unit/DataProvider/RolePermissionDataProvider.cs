using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public class RolePermissionDataProvider
{

    public static CreateRolePermissionCommand Create(string name = "RolePermission-name",
        string slug = "RolePermission-slug", long permissionId = 1, long roleId = 1)
        => new()
        {
            name = name,
            slug = slug,
            permission_id = permissionId,
            role_id = roleId
        };

    public static EditRolePermissionCommand Edit(string name = "name", long id = 1,
        string slug = "slug", long permissionId = 1, long roleId = 1)
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
            permission_id = permissionId,
            role_id = roleId
        };


    public static RolePermission Row(string name = "name", long id = 1, string slug = "slug", long permissionId = 1, long roleId = 1)
        => new()
        {
            id = id,
            name = name,
            slug = slug,
            updated_at = DateTime.UtcNow,
            permission_id = permissionId,
            role_id = roleId
        };

    public static DeleteRolePermissionCommand Delete(long id = 1)
        => new()
        {
            id = id,
        };

    public static GetRolePermissionBySlugQuery GetBySlug(string slug = "slug")
        => new()
        {
            slug = slug,
        };

    public static GetPaginatedRolePermissionQuery GetByQueryFilter(string searchTerm = "")
        => new()
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = searchTerm,
        };

    public static PaginatedList<RolePermission> GetPaginatedList()
        => new(new List<RolePermission>
            {
                new () { id = 1, name = "Tech" },
                new () { id = 2, name = "Health" }
            }
            , count: 2, page: 1, pageSize: 10);
}
