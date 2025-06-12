using System.Linq.Expressions;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.IBaseRepository;
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
		var roleRepoMock = new Mock<IRepository<Role>>();
		roleRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
			.ReturnsAsync(true);
		var permissionRepoMock = new Mock<IRepository<Permission>>();
		permissionRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
			.ReturnsAsync(true);
		await CreateHandlerTestHelper.TestCreateSuccess<
			CreateRolePermissionCommand,                 // Command Type
			RolePermission,                          // Entity Type
			IRepository<RolePermission>,               // Repository Interface
			CreateRolePermissionCommandHandler           // Handler Type
		>(
			handlerFactory: uow => new CreateRolePermissionCommandHandler(uow),
			execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
			command: RolePermissionDataProvider.Create(),
			repoSelector: uow => uow.RolePermissionRepository,
			setupUowMock: uow =>
			{
				uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
				uow.Setup(x => x.PermissionRepository).Returns(permissionRepoMock.Object);
			}
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
			IRepository<RolePermission>,
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
