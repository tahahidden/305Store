using Core.EntityFramework.Models;
using DataLayer.Services;
using GoldAPI.Application.AdminUserFeatures.Command;
using GoldAPI.Application.AdminUserFeatures.Handler;
using GoldAPI.Test.DataProvider;
using GoldAPI.Test.GenericHandlers;

namespace GoldAPI.Test.TestHandlers.AdminUserTests;
public class DeleteAdminUserCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldDeleteAdminUser_WhenExists()
	{
		var command = AdminUserDataProvider.Delete();

		await DeleteHandlerTestHelper.TestDelete<
			DeleteAdminUserCommand,
			User,
			IUserRepo,
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
			IUserRepo,
			DeleteAdminUserCommandHandler>(
			handlerFactory: uow => new DeleteAdminUserCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.UserRepository
		);
	}
}
