using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Handler;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Core.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class EditBlogCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEditBlog_WhenEntityExists()
    {
        await EditHandlerTestHelper.TestEditSuccess<EditBlogCommand,
            Blog,
            EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo),
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
        await EditHandlerTestHelper.TestEditNotFound<EditBlogCommand, Blog, EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogDataProvider.Edit(id: 2),
            entityId: 2
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
    {
        await EditHandlerTestHelper.TestEditCommitFail<EditBlogCommand, Blog, EditBlogCommandHandler>(
            handlerFactory: (repo, uow) => new EditBlogCommandHandler(uow, repo),
            execute: (handler, command, token) => handler.Handle(command, token),
            command: BlogDataProvider.Edit(name: "Updated Name", id: 1),
            entityId: 1,
            existingEntity: BlogDataProvider.Row(name: "old Name", id: 1)
        );
    }
}
