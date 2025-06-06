using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogCategoryTests;
public class DeleteCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenExists()
    {
        var command = BlogCategoryDataProvider.Delete();

        await DeleteHandlerTestHelper.TestDelete<
            DeleteCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            DeleteCategoryCommandHandler>(
            handlerFactory: uow => new DeleteCategoryCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogCategoryRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        var command = BlogCategoryDataProvider.Delete(id: 99);

        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeleteCategoryCommand,
            BlogCategory,
            IBlogCategoryRepository,
            DeleteCategoryCommandHandler>(
            handlerFactory: uow => new DeleteCategoryCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogCategoryRepository
        );
    }

}
