using System;
using System.Collections.Generic;
using System.Text;
using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public class PermissionDataProvider
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
			updated_at = DateTime.Now,
		};


	public static Permission Row(string name = "name", long id = 1, string slug = "slug")
		=> new()
		{
			id = id,
			name = name,
			slug = slug,
			updated_at = DateTime.Now,
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
		=> new(new List<Permission>
			{
				new () { id = 1, name = "Tech" },
				new () { id = 2, name = "Health" }
			}
			, count: 2, page: 1, pageSize: 10);
}
