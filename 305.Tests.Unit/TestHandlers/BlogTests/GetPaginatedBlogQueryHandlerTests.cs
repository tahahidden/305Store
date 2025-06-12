using _305.Application.Features.BlogFeatures.Handler;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class GetPaginatedBlogQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
	{
		var paginatedList = BlogDataProvider.GetPaginatedList();

		var query = BlogDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IRepository<Blog>,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList
		// ❌ includes رو حذف کن چون در نسخه جدید mock با includeFunc کار می‌کنه
		);
	}


	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var categories = new List<Blog>
	{
		BlogDataProvider.Row(id: 1, name: "Health"),
		BlogDataProvider.Row(id: 1, name: "Tech")
	};

		var paginatedList = new PaginatedList<Blog>(
			categories.Where(c => c.name.Contains("Tech")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = BlogDataProvider.GetByQueryFilter(searchTerm: "Tech");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IRepository<Blog>,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList
		// حذف includes چون الان به شکل Func<IQueryable<TEntity>, IQueryable<TEntity>> مدیریت می‌شود
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoBlogExists()
	{
		var paginatedList = new PaginatedList<Blog>(new List<Blog>(), 0, 1, 10);

		var query = BlogDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Blog,
			IRepository<Blog>,
			GetPaginatedBlogQueryHandler,
			GetPaginatedBlogQuery>(
				uow => new GetPaginatedBlogQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.BlogRepository,
				query,
				paginatedList
		// includes حذف شده به دلیل تغییر ساختار
		);
	}
}
