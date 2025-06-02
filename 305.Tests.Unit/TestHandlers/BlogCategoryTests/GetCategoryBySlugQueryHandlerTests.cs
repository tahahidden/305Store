using _305.Application.Features.BlogCategoryFeatures.Handler;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogCategoryTests;
public class GetCategoryBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenCategoryExists()
    {
        var category = BlogCategoryDataProvider.Row(name: "Name", id: 1, slug: "slug");


        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            BlogCategory,
            BlogCategoryResponse,
            IBlogCategoryRepository,
            GetCategoryBySlugQueryHandler>(
                uow => new GetCategoryBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(BlogCategoryDataProvider.GetBySlug(slug: "slug"), token),
                uow => uow.BlogCategoryRepository,
                category
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            BlogCategory,
            BlogCategoryResponse,
            IBlogCategoryRepository,
            GetCategoryBySlugQueryHandler>(
                uow => new GetCategoryBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(BlogCategoryDataProvider.GetBySlug(slug: "not-found"), token),
                uow => uow.BlogCategoryRepository
        );
    }
}

