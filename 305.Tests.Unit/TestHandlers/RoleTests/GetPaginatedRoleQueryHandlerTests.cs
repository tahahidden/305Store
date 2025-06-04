using _305.Application.Features.RoleFeatures.Handler;
using _305.Application.Features.RoleFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class GetPaginatedRoleQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
	{
		var paginatedList = RoleDataProvider.GetPaginatedList();

		var query = RoleDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Role,
			IRoleRepository,
			GetPaginatedRoleQueryHandler,
			GetPaginatedRoleQuery>(
				uow => new GetPaginatedRoleQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RoleRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var categories = new List<Role>
		{
			RoleDataProvider.Row(id: 1, name: "Admin"),
			RoleDataProvider.Row(id: 1, name: "God")
		};

		var paginatedList = new PaginatedList<Role>(
			categories.Where(c => c.name.Contains("God")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = RoleDataProvider.GetByQueryFilter(searchTerm: "God");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Role,
			IRoleRepository,
			GetPaginatedRoleQueryHandler,
			GetPaginatedRoleQuery>(
				uow => new GetPaginatedRoleQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RoleRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoRoleExists()
	{
		var paginatedList = new PaginatedList<Role>(new List<Role>(), 0, 1, 10);

		var query = RoleDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Role,
			IRoleRepository,
			GetPaginatedRoleQueryHandler,
			GetPaginatedRoleQuery>(
				uow => new GetPaginatedRoleQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RoleRepository,
				query,
				paginatedList
		);
	}
}
