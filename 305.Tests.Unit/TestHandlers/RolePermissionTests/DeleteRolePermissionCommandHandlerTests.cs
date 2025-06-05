using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RolePermissionTests;
public class DeleteRolePermissionCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldDeleteRolePermission_WhenExists()
	{
		var command = RolePermissionDataProvider.Delete();

		await DeleteHandlerTestHelper.TestDelete<
			DeleteRolePermissionCommand,
			RolePermission,
			IRolePermissionRepository,
			DeleteRolePermissionCommandHandler>(
			handlerFactory: uow => new DeleteRolePermissionCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.RolePermissionRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenRolePermissionDoesNotExist()
	{
		var command = RolePermissionDataProvider.Delete(id: 99);

		await DeleteHandlerTestHelper.TestDeleteNotFound<
			DeleteRolePermissionCommand,
			RolePermission,
			IRolePermissionRepository,
			DeleteRolePermissionCommandHandler>(
			handlerFactory: uow => new DeleteRolePermissionCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.RolePermissionRepository
		);
	}
}
