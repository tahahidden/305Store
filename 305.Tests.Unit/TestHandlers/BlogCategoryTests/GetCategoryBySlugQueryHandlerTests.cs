using _304.Net.Platform.Application.BlogCategoryFeatures.Handler;
using _304.Net.Platform.Application.BlogCategoryFeatures.Query;
using _304.Net.Platform.Application.BlogCategoryFeatures.Response;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Core.EntityFramework.Models;
using DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

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

