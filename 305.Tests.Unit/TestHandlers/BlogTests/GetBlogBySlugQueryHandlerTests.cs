using _304.Net.Platform.Application.BlogFeatures.Handler;
using _304.Net.Platform.Application.BlogFeatures.Response;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
                blog,
                includes: new[] { "blog_category" } // 👈 حتماً اگر هندلر include داره، اینجا هم بدی
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
