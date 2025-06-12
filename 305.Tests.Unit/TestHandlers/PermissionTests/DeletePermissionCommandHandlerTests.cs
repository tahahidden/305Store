using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class DeletePermissionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeletePermission_WhenExists()
    {
        var command = PermissionDataProvider.Delete();

        await DeleteHandlerTestHelper.TestDelete<
            DeletePermissionCommand,
            Permission,
            IRepository<Permission>,
            DeletePermissionCommandHandler>(
            handlerFactory: uow => new DeletePermissionCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.PermissionRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        var command = PermissionDataProvider.Delete(id: 99);

        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeletePermissionCommand,
            Permission,
            IRepository<Permission>,
            DeletePermissionCommandHandler>(
            handlerFactory: uow => new DeletePermissionCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.PermissionRepository
        );
    }
}
