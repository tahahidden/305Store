using _305.Application.Features.RoleFeatures.Handler;
using _305.Application.Features.RoleFeatures.Query;
using _305.Application.Features.RoleFeatures.Response;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.RoleTests;
public class GetAllRoleQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnList_WhenCategoriesExist()
	{
		var categories = new List<Role>
		{
			RoleDataProvider.Row(name: "Name 1", id: 1),
			RoleDataProvider.Row(name: "Name 2", id: 2)
		};

		await GetAllHandlerTestHelper.TestHandle_Success
			<Role, RoleResponse, IRepository<Role>, GetAllRoleQueryHandler>(
				handlerFactory: unitOfWork => new GetAllRoleQueryHandler(unitOfWork),
				execute: (handler, ct) => handler.Handle(new GetAllRoleQuery(), ct),
				repoSelector: u => u.RoleRepository,
				entities: categories);
	}

	[Fact]
	public async Task Handle_ShouldReturnFail_WhenExceptionThrown()
	{
		await GetAllHandlerTestHelper.TestHandle_FailOnException<Role, RoleResponse, IRepository<Role>, GetAllRoleQueryHandler>(
			handlerFactory: uow => new GetAllRoleQueryHandler(uow),
			execute: (handler, ct) => handler.Handle(new GetAllRoleQuery(), ct),
			repoSelector: uow => uow.RoleRepository);
	}
}
