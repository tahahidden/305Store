using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.Features.RolePermissionFeatures.Response;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RolePermissionTests;
public class GetRolePermissionBySlugQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnData_WhenRolePermissionExists()
	{
		var RolePermission = RolePermissionDataProvider.Row(name: "Name", id: 1, slug: "slug");


		await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
			RolePermission,
			RolePermissionResponse,
			IRepository<RolePermission>,
			GetRolePermissionBySlugQueryHandler>(
			uow => new GetRolePermissionBySlugQueryHandler(uow),
			(handler, token) => handler.Handle(RolePermissionDataProvider.GetBySlug(slug: "slug"), token),
			uow => uow.RolePermissionRepository,
			RolePermission
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenRolePermissionDoesNotExist()
	{
		await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
			RolePermission,
			RolePermissionResponse,
			IRepository<RolePermission>,
			GetRolePermissionBySlugQueryHandler>(
			uow => new GetRolePermissionBySlugQueryHandler(uow),
			(handler, token) => handler.Handle(RolePermissionDataProvider.GetBySlug(slug: "not-found"), token),
			uow => uow.RolePermissionRepository
		);
	}
}
