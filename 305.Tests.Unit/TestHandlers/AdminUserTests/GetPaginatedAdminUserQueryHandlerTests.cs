using _305.Application.Features.AdminUserFeatures.Handler;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.AdminUserTests;
public class GetPaginatedAdminUserQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenAdminUserExist()
	{
		var paginatedList = AdminUserDataProvider.GetPaginatedList();

		var query = AdminUserDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			User,
			IUserRepository,
			GetPaginatedAdminUserQueryHandler,
			GetPaginatedAdminUserQuery>(
				uow => new GetPaginatedAdminUserQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.UserRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var AdminUser = new List<User>
		{
			AdminUserDataProvider.Row(id: 1, name: "Health"),
			AdminUserDataProvider.Row(id: 1, name: "Tech")
		};

		var paginatedList = new PaginatedList<User>(
			AdminUser.Where(c => c.name.Contains("Tech")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = AdminUserDataProvider.GetByQueryFilter(searchTerm: "Tech");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			User,
			IUserRepository,
			GetPaginatedAdminUserQueryHandler,
			GetPaginatedAdminUserQuery>(
				uow => new GetPaginatedAdminUserQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.UserRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoAdminUserExists()
	{
		var paginatedList = new PaginatedList<User>(new List<User>(), 0, 1, 10);

		var query = AdminUserDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			User,
			IUserRepository,
			GetPaginatedAdminUserQueryHandler,
			GetPaginatedAdminUserQuery>(
				uow => new GetPaginatedAdminUserQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.UserRepository,
				query,
				paginatedList
		);
	}
}
