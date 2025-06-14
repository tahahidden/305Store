using _305.Application.Features.PermissionFeatures.Handler;
using _305.Application.Features.PermissionFeatures.Response;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class GetPermissionBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenPermissionExists()
    {
        var permission = PermissionDataProvider.Row(name: "Name", id: 1, slug: "slug");


        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            Permission,
            PermissionResponse,
            IRepository<Permission>,
            GetPermissionBySlugQueryHandler>(
            uow => new GetPermissionBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(PermissionDataProvider.GetBySlug(slug: "slug"), token),
            uow => uow.PermissionRepository,
            permission
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            Permission,
            PermissionResponse,
            IRepository<Permission>,
            GetPermissionBySlugQueryHandler>(
            uow => new GetPermissionBySlugQueryHandler(uow),
            (handler, token) => handler.Handle(PermissionDataProvider.GetBySlug(slug: "not-found"), token),
            uow => uow.PermissionRepository
        );
    }
}
