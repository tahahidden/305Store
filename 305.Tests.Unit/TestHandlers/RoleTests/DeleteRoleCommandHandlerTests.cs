using _305.Application.Features.RoleFeatures.Command;
using _305.Application.Features.RoleFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class DeleteRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteRole_WhenExists()
    {
        var command = RoleDataProvider.Delete();

        await DeleteHandlerTestHelper.TestDelete<
            DeleteRoleCommand,
            Role,
            IRepository<Role>,
            DeleteRoleCommandHandler>(
            handlerFactory: uow => new DeleteRoleCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.RoleRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        var command = RoleDataProvider.Delete(id: 99);

        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeleteRoleCommand,
            Role,
            IRepository<Role>,
            DeleteRoleCommandHandler>(
            handlerFactory: uow => new DeleteRoleCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.RoleRepository
        );
    }
}
