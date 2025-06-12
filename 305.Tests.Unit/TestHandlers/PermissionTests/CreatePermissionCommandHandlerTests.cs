using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class CreatePermissionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreatePermission_WhenNameAndSlugAreUnique()
    {
        await CreateHandlerTestHelper.TestCreateSuccess<
            CreatePermissionCommand,                 // Command Type
            Permission,                          // Entity Type
            IRepository<Permission>,               // Repository Interface
            CreatePermissionCommandHandler           // Handler Type
        >(
            handlerFactory: uow => new CreatePermissionCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: PermissionDataProvider.Create(),
            repoSelector: uow => uow.PermissionRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        // Arrange
        var command = PermissionDataProvider.Create();

        await CreateHandlerTestHelper.TestCreateException<
            CreatePermissionCommand,
            Permission,
            IRepository<Permission>,
            CreatePermissionCommandHandler>(

            handlerFactory: uow => new CreatePermissionCommandHandler(uow),

            execute: (handler, cmd, token) => handler.Handle(cmd, token),

            command: command,

            repoSelector: uow => uow.PermissionRepository,

            setupRepoMock: repoMock =>
            {
                // نام تکراری نیست
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
                    .ReturnsAsync(false);

                // شبیه‌سازی Exception هنگام Add
                repoMock.Setup(r => r.AddAsync(It.IsAny<Permission>()))
                    .ThrowsAsync(new Exception("DB error"));
            }
        );
    }
}
