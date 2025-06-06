using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Handler;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogCategoryTests;
public class EditCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditCategory_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess<EditCategoryCommand,
            BlogCategory,
            EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo), // فقط IUnitOfWork پاس می‌دهیم
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogCategoryDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: BlogCategoryDataProvider.Row(name: "old", id: 1),
            assertUpdated: entity =>
            {
                Assert.Equal("Updated Name", entity.name);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        await EditHandlerTestHelper.TestEditNotFound<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogCategoryDataProvider.Edit(id: 2),
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail<EditCategoryCommand, BlogCategory, EditCategoryCommandHandler>(
            handlerFactory: (repo, uow) => new EditCategoryCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogCategoryDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: BlogCategoryDataProvider.Row(name: "old Name", id: 1)
        );
    }
}