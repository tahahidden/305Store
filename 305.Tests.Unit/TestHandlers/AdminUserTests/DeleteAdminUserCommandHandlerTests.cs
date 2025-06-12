using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.Features.AdminUserFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.AdminUserTests;
public class DeleteAdminUserCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldDeleteAdminUser_WhenExists()
	{
		var command = AdminUserDataProvider.Delete();

		await DeleteHandlerTestHelper.TestDelete<
			DeleteAdminUserCommand,
			User,
			IRepository<User>,
			DeleteAdminUserCommandHandler>(
			handlerFactory: uow => new DeleteAdminUserCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.UserRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenAdminUserDoesNotExist()
	{
		var command = AdminUserDataProvider.Delete(id: 99);

		await DeleteHandlerTestHelper.TestDeleteNotFound<
			DeleteAdminUserCommand,
			User,
			IRepository<User>,
			DeleteAdminUserCommandHandler>(
			handlerFactory: uow => new DeleteAdminUserCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.UserRepository
		);
	}
}
