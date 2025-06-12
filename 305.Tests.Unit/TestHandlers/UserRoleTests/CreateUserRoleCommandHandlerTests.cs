using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class CreateUserRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateUserRole_WhenNameAndSlugAreUnique()
    {
        var roleRepoMock = new Mock<IRepository<Role>>();
        roleRepoMock
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(true);

        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateUserRoleCommand,                 // Command Type
            UserRole,                          // Entity Type
            IRepository<UserRole>,               // Repository Interface
            CreateUserRoleCommandHandler           // Handler Type
        >(
            handlerFactory: uow => new CreateUserRoleCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: UserRoleDataProvider.Create(),
            repoSelector: uow => uow.UserRoleRepository,
            setupUowMock: uow =>
            {
                uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
                uow.Setup(x => x.UserRepository).Returns(userRepoMock.Object);
            }
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
            IRepository<UserRole>,
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
