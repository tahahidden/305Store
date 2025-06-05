using System.Linq.Expressions;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;

namespace _305.Tests.Unit.TestHandlers.RolePermissionTests;
public class CreateRolePermissionCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldCreateRolePermission_WhenNameAndSlugAreUnique()
	{
		await CreateHandlerTestHelper.TestCreateSuccess<
			CreateRolePermissionCommand,                 // Command Type
			RolePermission,                          // Entity Type
			IRolePermissionRepository,               // Repository Interface
			CreateRolePermissionCommandHandler           // Handler Type
		>(
			handlerFactory: uow => new CreateRolePermissionCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: RolePermissionDataProvider.Create(),
			repoSelector: uow => uow.RolePermissionRepository
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnError_WhenExceptionThrown()
	{
		// Arrange
		var command = RolePermissionDataProvider.Create();

		await CreateHandlerTestHelper.TestCreateException<
			CreateRolePermissionCommand,
			RolePermission,
			IRolePermissionRepository,
			CreateRolePermissionCommandHandler>(

			handlerFactory: uow => new CreateRolePermissionCommandHandler(uow),

			execute: (handler, cmd, token) => handler.Handle(cmd, token),

			command: command,

			repoSelector: uow => uow.RolePermissionRepository,

			setupRepoMock: repoMock =>
			{
				// نام تکراری نیست
				repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<RolePermission, bool>>>()))
					.ReturnsAsync(false);

				// شبیه‌سازی Exception هنگام Add
				repoMock.Setup(r => r.AddAsync(It.IsAny<RolePermission>()))
					.ThrowsAsync(new Exception("DB error"));
			}
		);
	}
}
