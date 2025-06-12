using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Handler;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class EditPermissionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditPermission_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess(
            handlerFactory: (repo, uow) => new EditPermissionCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: PermissionDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: PermissionDataProvider.Row(name: "old", id: 1),
            assertUpdated: entity =>
            {
                Assert.Equal("Updated Name", entity.name);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        await EditHandlerTestHelper.TestEditNotFound<EditPermissionCommand, Permission, EditPermissionCommandHandler>(
            handlerFactory: (repo, uow) => new EditPermissionCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: PermissionDataProvider.Edit(id: 2),
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail(
            handlerFactory: (repo, uow) => new EditPermissionCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: PermissionDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: PermissionDataProvider.Row(name: "old Name", id: 1)
        );
    }
}
