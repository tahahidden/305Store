using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Handler;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class EditBlogCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditBlog_WhenEntityExists()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("images/test.jpg");
        await EditHandlerTestHelper.TestEditSuccess<EditBlogCommand,
            Blog,
            EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo, fileServiceMock.Object),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: BlogDataProvider.Row(name: "old", id: 1),
            assertUpdated: entity =>
            {
                Assert.Equal("Updated Name", entity.name);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await EditHandlerTestHelper.TestEditNotFound<EditBlogCommand, Blog, EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo, fileServiceMock.Object),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogDataProvider.Edit(id: 2),
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");

        await EditHandlerTestHelper.TestEditCommitFail<EditBlogCommand, Blog, EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo, fileServiceMock.Object),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: BlogDataProvider.Row(name: "old Name", id: 1)
        );
    }
}
