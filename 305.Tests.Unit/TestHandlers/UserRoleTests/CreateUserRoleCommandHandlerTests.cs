using _305.Tests.Unit.DataProvider;
using Core.EntityFramework.Models;
using DataLayer.Services;
using GoldAPI.Application.UserRoleFeatures.Command;
using GoldAPI.Application.UserRoleFeatures.Handler;
using GoldAPI.Test.GenericHandlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class CreateUserRoleCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldCreateUserRole_WhenNameAndSlugAreUnique()
	{
		await CreateHandlerTestHelper.TestCreateSuccess<
			CreateUserRoleCommand,                 // Command Type
			UserRole,                          // Entity Type
			IUserRoleRepo,               // Repository Interface
			CreateUserRoleCommandHandler           // Handler Type
		>(
			handlerFactory: uow => new CreateUserRoleCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: UserRoleDataProvider.Create(),
			repoSelector: uow => uow.UserRoleRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnError_WhenExceptionThrown()
	{
		// Arrange
		var command = UserRoleDataProvider.Create();

		await CreateHandlerTestHelper.TestCreateException<
			CreateUserRoleCommand,
			UserRole,
			IUserRoleRepo,
			CreateUserRoleCommandHandler>(

			handlerFactory: uow => new CreateUserRoleCommandHandler(uow),

			execute: (handler, cmd, token) => handler.Handle(cmd, token),

			command: command,

			repoSelector: uow => uow.UserRoleRepository,

			setupRepoMock: repoMock =>
			{
				// نام تکراری نیست
				repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<UserRole, bool>>>()))
					.ReturnsAsync(false);

				// شبیه‌سازی Exception هنگام Add
				repoMock.Setup(r => r.AddAsync(It.IsAny<UserRole>()))
					.ThrowsAsync(new Exception("DB error"));
			}
		);
	}
}
