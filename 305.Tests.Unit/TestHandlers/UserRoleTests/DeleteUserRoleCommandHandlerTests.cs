using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class DeleteUserRoleCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldDeleteUserRole_WhenExists()
	{
		var command = UserRoleDataProvider.Delete();

		await DeleteHandlerTestHelper.TestDelete<
			DeleteUserRoleCommand,
			UserRole,
			IUserRoleRepository,
			DeleteUserRoleCommandHandler>(
			handlerFactory: uow => new DeleteUserRoleCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.UserRoleRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenUserRoleDoesNotExist()
	{
		var command = UserRoleDataProvider.Delete(id: 99);

		await DeleteHandlerTestHelper.TestDeleteNotFound<
			DeleteUserRoleCommand,
			UserRole,
			IUserRoleRepository,
			DeleteUserRoleCommandHandler>(
			handlerFactory: uow => new DeleteUserRoleCommandHandler(uow),
			execute: (handler, cmd, token) => handler.Handle(cmd, token),
			command: command,
			repoSelector: uow => uow.UserRoleRepository
		);
	}
}
