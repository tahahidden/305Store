using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

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
