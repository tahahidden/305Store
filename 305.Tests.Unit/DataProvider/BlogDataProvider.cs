using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Features.BlogFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;
using _305.Tests.Unit.Assistant;

namespace _305.Tests.Unit.DataProvider;
internal static class BlogDataProvider
{
	public static CreateBlogCommand Create(string name = "test")
		=> new CreateBlogCommand()
		{
			name = name,
			description = "Test",
			image_alt = "Test",
			image_file = Files.CreateFakeFormFile(),
			blog_category_id = 1,
			blog_text = "Test",
			created_at = DateTime.Now,
			updated_at = DateTime.Now,
			estimated_read_time = 5,
			keywords = "a,b,c",
			show_blog = true,
			meta_description = "Test",
			slug = null,
		};

	public static EditBlogCommand Edit(string name = "test", long id = 1)
	=> new EditBlogCommand()
	{
		id = id,
		name = name,
		description = "Test",
		image_alt = "Test",
		image_file = Files.CreateFakeFormFile(),
		blog_category_id = 1,
		blog_text = "Test",
		updated_at = DateTime.Now,
		estimated_read_time = 5,
		keywords = "a,b,c",
		show_blog = true,
		meta_description = "Test",
		slug = null,
		image = null,
	};


	public static Blog Row(string name = "name", long id = 1, string slug = "slug", string image = "image.jpg")
	=> new Blog()
	{
		id = id,
		name = name,
		description = "Test",
		image_alt = "Test",
		blog_category_id = 1,
		blog_text = "Test",
		updated_at = DateTime.Now,
		estimated_read_time = 5,
		keywords = "a,b,c",
		show_blog = true,
		meta_description = "Test",
		slug = slug,
		image = image,
		created_at = DateTime.Now,
		blog_category = BlogCategoryDataProvider.Row()
	};

	public static DeleteBlogCommand Delete(long id = 1)
		=> new DeleteBlogCommand()
		{
			id = id,
		};

	public static GetBlogBySlugQuery GetBySlug(string slug = "slug")
	=> new GetBlogBySlugQuery()
	{
		slug = slug,
	};

	public static BlogResponse GetOne(string slug = "slug", string name = "name")
		=> new BlogResponse()
		{
			id = 1,
			name = name,
			description = "Test",
			image_alt = "Test",
			blog_category_id = 1,
			blog_text = "Test",
			updated_at = DateTime.Now,
			estimated_read_time = 5,
			keywords = "a,b,c",
			show_blog = true,
			meta_description = "Test",
			slug = slug,
			image = "test.jpg",
			created_at = DateTime.Now,
			blog_category = BlogCategoryDataProvider.GetOne()
		};

	public static GetPaginatedBlogQuery GetByQueryFilter(string searchTerm = "")
	=> new GetPaginatedBlogQuery()
	{
		Page = 1,
		PageSize = 10,
		SearchTerm = searchTerm,
	};

	public static PaginatedList<Blog> GetPaginatedList()
	=> new PaginatedList<Blog>(new List<Blog>
		{
			new Blog
			{id = 1,
			name = "slug 1",
			description = "Test",
			image_alt = "Test",
			blog_category_id = 1,
			blog_text = "Test",
			updated_at = DateTime.Now,
			estimated_read_time = 5,
			keywords = "a,b,c",
			show_blog = true,
			meta_description = "Test",
			slug = "slug-1",
			image = "test.jpg",
			created_at = DateTime.Now,
			blog_category = BlogCategoryDataProvider.Row()
			},
			new Blog
			{id = 1,
			name = "slug 2",
			description = "Test",
			image_alt = "Test",
			blog_category_id = 1,
			blog_text = "Test",
			updated_at = DateTime.Now,
			estimated_read_time = 5,
			keywords = "a,b,c",
			show_blog = true,
			meta_description = "Test",
			slug = "slug-2",
			image = "test.jpg",
			created_at = DateTime.Now,
			blog_category = BlogCategoryDataProvider.Row()
			}
		}
	, count: 2, page: 1, pageSize: 10);

}



