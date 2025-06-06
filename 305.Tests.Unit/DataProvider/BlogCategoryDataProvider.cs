using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class BlogCategoryDataProvider
{
	public static CreateCategoryCommand Create(string name = "name", string slug = "slug")
		=> new ()
		{
			name = name,
			created_at = DateTime.Now,
			description = "description",
			slug = slug,
			updated_at = DateTime.Now,
		};

	public static EditCategoryCommand Edit(string name = "name", long id = 1, string slug = "slug")
		=> new ()
		{
			id = id,
			name = name,
			description = "description",
			slug = slug,
			updated_at = DateTime.Now,
		};


	public static BlogCategory Row(string name = "name", long id = 1, string slug = "slug")
	=> new ()
	{
		id = id,
		name = name,
		description = "description",
		slug = slug,
		updated_at = DateTime.Now,
	};

	public static DeleteCategoryCommand Delete(long id = 1)
		=> new ()
		{
			id = id,
		};

	public static GetCategoryBySlugQuery GetBySlug(string slug = "slug")
	=> new ()
	{
		slug = slug,
	};

	public static BlogCategoryResponse GetOne(string slug = "slug", string name = "name")
		=> new ()
		{
			id = 1,
			name = name,
			slug = slug,
			description = "Tech Category"
		};

	public static GetPaginatedCategoryQuery GetByQueryFilter(string searchTerm = "")
	=> new ()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<BlogCategory> GetPaginatedList()
	=> new (new List<BlogCategory>
		{
			new () { id = 1, name = "Tech" },
			new () { id = 2, name = "Health" }
		}
	, count: 2, page: 1, pageSize: 10);
}
