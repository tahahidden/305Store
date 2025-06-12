using _305.Application.Features.PermissionFeatures.Handler;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Filters.Pagination;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class GetPaginatedPermissionQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnPaginatedList_WhenCategoriesExist()
	{
		var paginatedList = PermissionDataProvider.GetPaginatedList();

		var query = PermissionDataProvider.GetByQueryFilter();

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Permission,
			IRepository<Permission>,
			GetPaginatedPermissionQueryHandler,
			GetPaginatedPermissionQuery>(
				uow => new GetPaginatedPermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.PermissionRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldFilterBySearchTerm_WhenSearchTermProvided()
	{
		// Arrange
		var categories = new List<Permission>
		{
			PermissionDataProvider.Row(id: 1, name: "Admin"),
			PermissionDataProvider.Row(id: 1, name: "God")
		};

		var paginatedList = new PaginatedList<Permission>(
			categories.Where(c => c.name.Contains("God")).ToList(),
			count: 1, page: 1, pageSize: 10
		);

		var query = PermissionDataProvider.GetByQueryFilter(searchTerm: "God");

		// Act + Assert
		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Permission,
			IRepository<Permission>,
			GetPaginatedPermissionQueryHandler,
			GetPaginatedPermissionQuery>(
				uow => new GetPaginatedPermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.PermissionRepository,
				query,
				paginatedList
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnEmptyList_WhenNoPermissionExists()
	{
		var paginatedList = new PaginatedList<Permission>(new List<Permission>(), 0, 1, 10);

		var query = PermissionDataProvider.GetByQueryFilter();

		await GetPaginatedHandlerTestHelper.TestPaginated_Success<
			Permission,
			IRepository<Permission>,
			GetPaginatedPermissionQueryHandler,
			GetPaginatedPermissionQuery>(
				uow => new GetPaginatedPermissionQueryHandler(uow),
				(handler, q, token) => handler.Handle(q, token),
				uow => uow.PermissionRepository,
				query,
				paginatedList
		);
	}
}
