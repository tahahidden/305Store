using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Handler;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class DeleteBlogCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteBlog_WhenExists()
    {
        var command = BlogDataProvider.Delete();

        await DeleteHandlerTestHelper.TestDelete<
            DeleteBlogCommand,
            Blog,
            IBlogRepository,
            DeleteBlogCommandHandler>(
            handlerFactory: uow => new DeleteBlogCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        var command = BlogDataProvider.Delete(id: 99);

        await DeleteHandlerTestHelper.TestDeleteNotFound<
            DeleteBlogCommand,
            Blog,
            IBlogRepository,
            DeleteBlogCommandHandler>(
            handlerFactory: uow => new DeleteBlogCommandHandler(uow),
            execute: (handler, cmd, token) => handler.Handle(cmd, token),
            command: command,
            repoSelector: uow => uow.BlogRepository
        );
    }
}
