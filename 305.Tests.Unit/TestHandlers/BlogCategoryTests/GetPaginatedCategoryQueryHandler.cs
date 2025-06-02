using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Core.EntityFramework.Models;
using Core.Pagination;
using DataLayer.Services;


namespace _305.Tests.Unit.TestHandlers.BlogCategoryTests;
public class GetPaginatedCategoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
    {
        var paginatedList = BlogCategoryDataProvider.GetPaginatedList();

        var query = BlogCategoryDataProvider.GetByQueryFilter();

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
    {
        // Arrange
        var categories = new List<BlogCategory>
        {
            BlogCategoryDataProvider.Row(id: 1, name: "Health"),
            BlogCategoryDataProvider.Row(id: 1, name: "Tech")
        };

        var paginatedList = new PaginatedList<BlogCategory>(
            categories.Where(c => c.name.Contains("Tech")).ToList(),
            count: 1, page: 1, pageSize: 10
        );

        var query = BlogCategoryDataProvider.GetByQueryFilter(searchTerm: "Tech");

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoryExists()
    {
        var paginatedList = new PaginatedList<BlogCategory>(new List<BlogCategory>(), 0, 1, 10);

        var query = BlogCategoryDataProvider.GetByQueryFilter();

        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            BlogCategory,
            IBlogCategoryRepository,
            GetPaginatedCategoryQueryHandler,
            GetPaginatedCategoryQuery>(
                uow => new GetPaginatedCategoryQueryHandler(uow),
                (handler, q, token) => handler.Handle(q, token),
                uow => uow.BlogCategoryRepository,
                query,
                paginatedList
        );
    }

}


