using _305.Application.Features.BlogFeatures.Handler;
using _305.Application.Features.BlogFeatures.Response;
using _305.Application.IRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class GetBlogBySlugQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnData_WhenBlogExists()
    {
        var blog = BlogDataProvider.Row(name: "Name", id: 1, slug: "slug");

        await GetBySlugHandlerTestHelper.TestGetBySlug_Success<
            Blog,
            BlogResponse,
            IBlogRepository,
            GetBlogBySlugQueryHandler>(
                uow => new GetBlogBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(BlogDataProvider.GetBySlug("slug"), token),
                uow => uow.BlogRepository,
                blog
        // دیگه نیازی به includes نیست چون mock با includeFunc تنظیم می‌شه
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBlogDoesNotExist()
    {
        await GetBySlugHandlerTestHelper.TestGetBySlug_NotFound<
            Blog,
            BlogResponse,
            IBlogRepository,
            GetBlogBySlugQueryHandler>(
                uow => new GetBlogBySlugQueryHandler(uow),
                (handler, token) => handler.Handle(BlogDataProvider.GetBySlug(slug: "not-found"), token),
                uow => uow.BlogRepository
        );
    }
}
