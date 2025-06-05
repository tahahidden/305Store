using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.Features.RolePermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.RolePermissionTests;
public class GetPaginatedRolePermissionQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
	{
		var paginatedList = RolePermissionDataProvider.GetPaginatedList();

		var query = RolePermissionDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			RolePermission,
			IRolePermissionRepository,
			GetPaginatedRolePermissionQueryHandler,
			GetPaginatedRolePermissionQuery>(
				uow => new GetPaginatedRolePermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RolePermissionRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var categories = new List<RolePermission>
		{
			RolePermissionDataProvider.Row(id: 1, name: "Admin"),
			RolePermissionDataProvider.Row(id: 1, name: "God")
		};

		var paginatedList = new PaginatedList<RolePermission>(
			categories.Where(c => c.name.Contains("God")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = RolePermissionDataProvider.GetByQueryFilter(searchTerm: "God");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			RolePermission,
			IRolePermissionRepository,
			GetPaginatedRolePermissionQueryHandler,
			GetPaginatedRolePermissionQuery>(
				uow => new GetPaginatedRolePermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RolePermissionRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoRolePermissionExists()
	{
		var paginatedList = new PaginatedList<RolePermission>(new List<RolePermission>(), 0, 1, 10);

		var query = RolePermissionDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			RolePermission,
			IRolePermissionRepository,
			GetPaginatedRolePermissionQueryHandler,
			GetPaginatedRolePermissionQuery>(
				uow => new GetPaginatedRolePermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.RolePermissionRepository,
				query,
				paginatedList
		);
	}
}
