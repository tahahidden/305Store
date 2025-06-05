using _305.Application.Features.PermissionFeatures.Handler;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Features.PermissionFeatures.Response;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.PermissionTests;
public class GetAllPermissionQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnList_WhenCategoriesExist()
	{
		var categories = new List<Permission>
		{
			PermissionDataProvider.Row(name: "Name 1", id: 1),
			PermissionDataProvider.Row(name: "Name 2", id: 2)
		};

		await GetAllHandlerTestHelper.TestHandle_Success
			<Permission, PermissionResponse, IPermissionRepository, GetAllPermissionQueryHandler>(
				handlerFactory: unitOfWork => new GetAllPermissionQueryHandler(unitOfWork),
				execute: (handler, ct) => handler.Handle(new GetAllPermissionQuery(), ct),
				repoSelector: u => u.PermissionRepository,
				entities: categories);
	}

	[Fact]
	public async Task Handle_ShouldReturnFail_WhenExceptionThrown()
	{
		await GetAllHandlerTestHelper.TestHandle_FailOnException<Permission, PermissionResponse, IPermissionRepository, GetAllPermissionQueryHandler>(
			handlerFactory: uow => new GetAllPermissionQueryHandler(uow),
			execute: (handler, ct) => handler.Handle(new GetAllPermissionQuery(), ct),
			repoSelector: uow => uow.PermissionRepository);
	}
}
