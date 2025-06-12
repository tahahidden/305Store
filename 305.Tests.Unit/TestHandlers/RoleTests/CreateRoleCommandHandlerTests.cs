using _305.Application.Features.RoleFeatures.Command;
using _305.Application.Features.RoleFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class CreateRoleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateRole_WhenNameAndSlugAreUnique()
    {
        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateRoleCommand,                 // Command Type
            Role,                          // Entity Type
            IRepository<Role>,               // Repository Interface
            CreateRoleCommandHandler           // Handler Type
        >(
            handlerFactory: uow => new CreateRoleCommandHandler(uow),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: RoleDataProvider.Create(),
            repoSelector: uow => uow.RoleRepository
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        // Arrange
        var command = RoleDataProvider.Create();

        await CreateHandlerTestHelper.TestCreateException<
            CreateRoleCommand,
            Role,
            IRepository<Role>,
            CreateRoleCommandHandler>(

            handlerFactory: uow => new CreateRoleCommandHandler(uow),

            execute: (handler, cmd, token) => handler.Handle(cmd, token),

            command: command,

            repoSelector: uow => uow.RoleRepository,

            setupRepoMock: repoMock =>
            {
                // نام تکراری نیست
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                    .ReturnsAsync(false);

                // شبیه‌سازی Exception هنگام Add
                repoMock.Setup(r => r.AddAsync(It.IsAny<Role>()))
                    .ThrowsAsync(new Exception("DB error"));
            }
        );
    }
}
