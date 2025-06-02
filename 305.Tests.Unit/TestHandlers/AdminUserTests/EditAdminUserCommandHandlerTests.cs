using _305.Application.Features.AdminUserFeatures.Command;
using _305.Tests.Unit.DataProvider;
using Core.EntityFramework.Models;
using GoldAPI.Application.AdminUserFeatures.Handler;
using GoldAPI.Test.GenericHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.AdminUserTests;
public class EditAdminUserCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldEditAdminUser_WhenEntityExists()
	{
		await EditHandlerTestHelper.TestEditSuccess<EditAdminUserCommand,
			User,
			EditAdminUserCommandHandler>(
			handlerFactory: (repo, uow) => new EditAdminUserCommandHandler(uow, repo), // فقط IUnitOfWork پاس می‌دهیم
			execute: (handler, command, token) => handler.Handle(command, token),
			command: AdminUserDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,
			existingEntity: AdminUserDataProvider.Row(name: "old", id: 1),
			assertUpdated: entity =>
			{
				Assert.Equal("Updated Name", entity.name);
			}
		);
	}


	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
	{
		await EditHandlerTestHelper.TestEditNotFound<EditAdminUserCommand, User, EditAdminUserCommandHandler>(
			handlerFactory: (repo, uow) => new EditAdminUserCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: AdminUserDataProvider.Edit(id: 2),
			entityId: 2
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
	{
		await EditHandlerTestHelper.TestEditCommitFail<EditAdminUserCommand, User, EditAdminUserCommandHandler>(
			handlerFactory: (repo, uow) => new EditAdminUserCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: AdminUserDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,
			existingEntity: AdminUserDataProvider.Row(name: "old Name", id: 1)
		);
	}
}
