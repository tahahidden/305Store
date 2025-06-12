using _305.Application.Features.UserRoleFeatures.Handler;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class GetPaginatedUserRoleQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
    {
        var paginatedList = UserRoleDataProvider.GetPaginatedList();

        var query = UserRoleDataProvider.GetByQueryFilter();

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            UserRole,
            IRepository<UserRole>,
            GetPaginatedUserRoleQueryHandler,
            GetPaginatedUserRoleQuery>(
            uow => new GetPaginatedUserRoleQueryHandler(uow),
            (handler, q, token) => handler.Handle(q, token),
            uow => uow.UserRoleRepository,
            query,
            paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
    {
        // Arrange
        var categories = new List<UserRole>
        {
            UserRoleDataProvider.Row(id: 1, name: "Admin"),
            UserRoleDataProvider.Row(id: 1, name: "God")
        };

        var paginatedList = new PaginatedList<UserRole>(
            categories.Where(c => c.name.Contains("God")).ToList(),
            count: 1, page: 1, pageSize: 10
        );

        var query = UserRoleDataProvider.GetByQueryFilter(searchTerm: "God");

        // Act + Assert
        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            UserRole,
            IRepository<UserRole>,
            GetPaginatedUserRoleQueryHandler,
            GetPaginatedUserRoleQuery>(
            uow => new GetPaginatedUserRoleQueryHandler(uow),
            (handler, q, token) => handler.Handle(q, token),
            uow => uow.UserRoleRepository,
            query,
            paginatedList
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUserRoleExists()
    {
        var paginatedList = new PaginatedList<UserRole>(new List<UserRole>(), 0, 1, 10);

        var query = UserRoleDataProvider.GetByQueryFilter();

        await GetPaginatedHandlerTestHelper.TestPaginated_Success<
            UserRole,
            IRepository<UserRole>,
            GetPaginatedUserRoleQueryHandler,
            GetPaginatedUserRoleQuery>(
            uow => new GetPaginatedUserRoleQueryHandler(uow),
            (handler, q, token) => handler.Handle(q, token),
            uow => uow.UserRoleRepository,
            query,
            paginatedList
        );
    }
}
