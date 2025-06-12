using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class DeleteBlogCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteBlog_WhenExists()
    {
        var command = BlogDataProvider.Delete();
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await DeleteHandlerTestHelper.TestDelete<
            DeleteBlogCommand,
            Blog,
            IRepository<Blog>,
            DeleteBlogCommandHandler>(
            handlerFactory: uow => new DeleteBlogCommandHandler(uow, fileServiceMock.Object),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        var command = BlogDataProvider.Delete(id: 99);
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeleteBlogCommand,
            Blog,
            IRepository<Blog>,
            DeleteBlogCommandHandler>(
            handlerFactory: uow => new DeleteBlogCommandHandler(uow, fileServiceMock.Object),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogRepository
        );
    }
}
