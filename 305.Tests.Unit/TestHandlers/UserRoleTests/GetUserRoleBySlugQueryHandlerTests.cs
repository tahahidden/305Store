using _305.Application.Features.UserRoleFeatures.Handler;
using _305.Application.Features.UserRoleFeatures.Response;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class GetUserRoleBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenUserRoleExists()
    {
        var userRole = UserRoleDataProvider.Row(name: "Name", id: 1, slug: "slug");


        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            UserRole,
            UserRoleResponse,
            IRepository<UserRole>,
            GetUserRoleBySlugQueryHandler>(
            uow => new GetUserRoleBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(UserRoleDataProvider.GetBySlug(slug: "slug"), token),
            uow => uow.UserRoleRepository,
            userRole
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserRoleDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            UserRole,
            UserRoleResponse,
            IRepository<UserRole>,
            GetUserRoleBySlugQueryHandler>(
            uow => new GetUserRoleBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(UserRoleDataProvider.GetBySlug(slug: "not-found"), token),
            uow => uow.UserRoleRepository
        );
    }
}
