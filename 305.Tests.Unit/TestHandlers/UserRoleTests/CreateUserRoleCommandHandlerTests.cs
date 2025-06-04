using _305.Tests.Unit.DataProvider;
using Moq;
using System.Linq.Expressions;
using _305.Application.Features.RoleFeatures.Command;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.GenericHandlers;
using _305.Application.Features.RoleFeatures.Handler;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class CreateUserRoleCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldCreateUserRole_WhenNameAndSlugAreUnique()
	{
		await CreateHandlerTestHelper.TestCreateSuccess<
			CreateRoleCommand,                 // Command Type
			UserRole,                          // Entity Type
			IUserRoleRepository,               // Repository Interface
			CreateRoleCommandHandler           // Handler Type
		>(
			handlerFactory: uow => new CreateRoleCommandHandler(uow),
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
			CreateRoleCommand,
			UserRole,
			IUserRoleRepository,
			CreateRoleCommandHandler>(

			handlerFactory: uow => new CreateRoleCommandHandler(uow),

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
