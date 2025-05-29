using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;

namespace _305.Tests.Unit.DataProvider;
public static class BlogCategoryDataProvider
{
	public static CreateCategoryCommand Create(string name = "name")
		=> new CreateCategoryCommand()
		{
			name = name,
			created_at = DateTime.Now,
			description = "description",
			slug = null,
			updated_at = DateTime.Now,
		};

	public static EditCategoryCommand Edit(string name = "name", long id = 1)
		=> new EditCategoryCommand()
		{
			id = id,
			name = name,
			description = "description",
			slug = null,
			updated_at = DateTime.Now,
		};


	public static BlogCategory Row(string name = "name", long id = 1, string slug = "slug")
	=> new BlogCategory()
	{
		id = id,
		name = name,
		description = "description",
		slug = slug,
		updated_at = DateTime.Now,
	};

	public static DeleteCategoryCommand Delete(long id = 1)
		=> new DeleteCategoryCommand()
		{
			id = id,
		};

	public static GetCategoryBySlugQuery GetBySlug(string slug = "slug")
	=> new GetCategoryBySlugQuery()
	{
		slug = slug,
	};

	public static BlogCategoryResponse GetOne(string slug = "slug", string name = "name")
		=> new BlogCategoryResponse()
		{
			id = 1,
			name = name,
			slug = slug,
			description = "Tech Category"
		};

	public static GetPaginatedCategoryQuery GetByQueryFilter(string searchTerm = "")
	=> new GetPaginatedCategoryQuery()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<BlogCategory> GetPaginatedList()
	=> new PaginatedList<BlogCategory>(new List<BlogCategory>
		{
			new BlogCategory { id = 1, name = "Tech" },
			new BlogCategory { id = 2, name = "Health" }
		}
	, count: 2, page: 1, pageSize: 10);
}
