using _305.Application.Features.AdminUserFeatures.Handler;
using _305.Application.Features.AdminUserFeatures.Response;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.AdminUserTests;
public class GetAdminUserBySlugQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnData_WhenAdminUserExists()
	{
		var AdminUser = AdminUserDataProvider.Row(name: "Name", id: 1, slug: "slug");


		await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
			User,
			AdminUserResponse,
			IUserRepository,
			GetAdminUserBySlugQueryHandler>(
				uow => new GetAdminUserBySlugQueryHandler(uow),
				(handler, token) => handler.Handle(AdminUserDataProvider.GetBySlug(slug: "slug"), token),
				uow => uow.UserRepository,
				AdminUser
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenAdminUserDoesNotExist()
	{
		await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
			User,
			AdminUserResponse,
			IUserRepository,
			GetAdminUserBySlugQueryHandler>(
				uow => new GetAdminUserBySlugQueryHandler(uow),
				(handler, token) => handler.Handle(AdminUserDataProvider.GetBySlug(slug: "not-found"), token),
				uow => uow.UserRepository
		);
	}
}
