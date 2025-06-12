using _305.Application.Features.RoleFeatures.Command;
using _305.Application.Features.RoleFeatures.Handler;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class EditRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditRole_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess(
            handlerFactory: (repo, uow) => new EditRoleCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: RoleDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: RoleDataProvider.Row(name: "old", id: 1),
            assertUpdated: entity =>
            {
                Assert.Equal("Updated Name", entity.name);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        await EditHandlerTestHelper.TestEditNotFound<EditRoleCommand, Role, EditRoleCommandHandler>(
            handlerFactory: (repo, uow) => new EditRoleCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: RoleDataProvider.Edit(id: 2),
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail(
            handlerFactory: (repo, uow) => new EditRoleCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: RoleDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: RoleDataProvider.Row(name: "old Name", id: 1)
        );
    }
}
