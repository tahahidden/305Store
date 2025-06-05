using _305.Application.Features.BlogCategoryFeatures.Handler;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogCategoryTests;
public class GetAllCategoryQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnList_WhenCategoriesExist()
	{
		var categories = new List<BlogCategory>
		{
			BlogCategoryDataProvider.Row(name: "Name 1", id: 1),
			BlogCategoryDataProvider.Row(name: "Name 2", id: 2)
		};

		await GetAllHandlerTestHelper.TestHandle_Success
			   <BlogCategory, BlogCategoryResponse, IBlogCategoryRepository, GetAllCategoryQueryHandler>(
			   handlerFactory: unitOfWork => new GetAllCategoryQueryHandler(unitOfWork),
			   execute: (handler, ct) => handler.Handle(new GetAllCategoryQuery(), ct),
			   repoSelector: u => u.BlogCategoryRepository,
			   entities: categories);
	}

	[Fact]
	public async Task Handle_ShouldReturnFail_WhenExceptionThrown()
	{
		await GetAllHandlerTestHelper.TestHandle_FailOnException<BlogCategory, BlogCategoryResponse, IBlogCategoryRepository, GetAllCategoryQueryHandler>(
			handlerFactory: uow => new GetAllCategoryQueryHandler(uow),
			execute: (handler, ct) => handler.Handle(new GetAllCategoryQuery(), ct),
			repoSelector: uow => uow.BlogCategoryRepository);
	}
}

