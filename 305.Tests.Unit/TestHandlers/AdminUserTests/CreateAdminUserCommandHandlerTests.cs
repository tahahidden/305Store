using _305.Application.Features.AdminUserFeatures.Command;
using _305.Tests.Unit.DataProvider;
using Moq;
using System.Linq.Expressions;
using _305.Application.Features.AdminUserFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.AdminUserTests;
public class CreateAdminUserCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldCreateAdminUser_WhenNameAndSlugAreUnique()
	{
		await CreateHandlerTestHelper.TestCreateSuccess<
			CreateAdminUserCommand,                 // Command Type
			User,                          // Entity Type
			IUserRepository,               // Repository Interface
			CreateAdminUserCommandHandler           // Handler Type
		>(
			handlerFactory: uow => new CreateAdminUserCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: AdminUserDataProvider.Create(),
			repoSelector: uow => uow.UserRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldFail_WhenNameIsDuplicate()
	{
		await CreateHandlerTestHelper.TestCreateFailure<
			CreateAdminUserCommand,
			User,                          // Entity Type
			IUserRepository,               // Repository Interface
			CreateAdminUserCommandHandler
		>(
			handlerFactory: uow => new CreateAdminUserCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: AdminUserDataProvider.Create(),
			repoSelector: uow => uow.UserRepository,
			setupRepoMock: repo =>
			{
				// name تکراری است => باید true برگرداند تا Valid نباشد
				repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
					.ReturnsAsync(true);
			},
			expectedMessage: null
		);
	}


	[Fact]
	public async Task Handle_ShouldFail_WhenSlugIsDuplicate()
	{
		await CreateHandlerTestHelper.TestCreateFailure<
			CreateAdminUserCommand,
			User,                          // Entity Type
			IUserRepository,               // Repository Interface
			CreateAdminUserCommandHandler
		>(
			handlerFactory: uow => new CreateAdminUserCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: AdminUserDataProvider.Create(),
			repoSelector: uow => uow.UserRepository,
			setupRepoMock: repo =>
			{
				repo.SetupSequence(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
					.ReturnsAsync(false) // برای name → یعنی name تکراری نیست
					.ReturnsAsync(true); // برای slug → یعنی slug تکراری است
			},
			expectedMessage: null
		);
	}



	[Fact]
	public async Task Handle_ShouldReturnError_WhenExceptionThrown()
	{
		// Arrange
		var command = AdminUserDataProvider.Create();

		await CreateHandlerTestHelper.TestCreateException<
			CreateAdminUserCommand,
			User,                          // Entity Type
			IUserRepository,               // Repository Interface
			CreateAdminUserCommandHandler>(

			handlerFactory: uow => new CreateAdminUserCommandHandler(uow),

			execute: (handler, cmd, token) => handler.Handle(cmd, token),

			command: command,

			repoSelector: uow => uow.UserRepository,

			setupRepoMock: repoMock =>
			{
				// نام تکراری نیست
				repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
					.ReturnsAsync(false);

				// شبیه‌سازی Exception هنگام Add
				repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
					.ThrowsAsync(new Exception("DB error"));
			}
		);
	}
}
