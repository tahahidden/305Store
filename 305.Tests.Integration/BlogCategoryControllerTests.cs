using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Response;
using _305.Tests.Integration;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace _305.IntegrationTests.Controllers;

[TestFixture]
public class BlogCategoryControllerTests
{
    private HttpClient _client = null!;
    private CustomWebApplicationFactory _factory = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Create_Should_Return_Success()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("test-title"), "name" },
            { new StringContent("test-title"), "slug" }
        };

        var response = await _client.PostAsync("/api/blog-category/create", form);
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Response Content: {content}");
    }

    [Test]
    public async Task Edit_Should_Return_Success()
    {
        // اول بساز
        var create = new MultipartFormDataContent
        {
            { new StringContent("edit-title"), "name" },
            { new StringContent("edit-slug"), "slug" }
        };
        var createResponse = await _client.PostAsync("/api/blog-category/create", create);
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(await createResponse.Content.ReadAsStringAsync());
        var id = createResult?.data;

        // حالا ویرایش
        var edit = new MultipartFormDataContent
        {
            { new StringContent(id!), "id" },
            { new StringContent("edited-title"), "name" },
            { new StringContent("edited-slug"), "slug" }
        };

        var response = await _client.PostAsync("/api/blog-category/edit", edit);
        var content = await response.Content.ReadAsStringAsync();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Response Content: {content}");
    }

    [Test]
    public async Task GetAll_Should_Return_List()
    {
        var response = await _client.GetAsync("/api/blog-category/all");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equals(result.Contains("data") || result.Contains("Data"), true);
    }

    [Test]
    public async Task Index_Paginated_Should_Return_Success()
    {
        var response = await _client.GetAsync("/api/blog-category/list?page=1&pageSize=10");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetBySlug_Should_Return_Success()
    {
        // ابتدا ایجاد
        var form = new MultipartFormDataContent
        {
            { new StringContent("slug-title"), "Title" },
            { new StringContent("slug-me"), "Slug" }
        };
        await _client.PostAsync("/api/blog-category/create", form);

        // درخواست با slug
        var slugForm = new MultipartFormDataContent
        {
            { new StringContent("slug-me"), "Slug" }
        };

        var response = await _client.PostAsync("/api/blog-category/get", slugForm);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Delete_Should_Return_Success()
    {
        // اول ایجاد
        var form = new MultipartFormDataContent
        {
            { new StringContent("delete-title"), "Title" },
            { new StringContent("delete-slug"), "Slug" }
        };
        var createResp = await _client.PostAsync("/api/blog-category/create", form);
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(await createResp.Content.ReadAsStringAsync());
        var id = createResult?.data;

        // حالا حذف
        var deleteForm = new MultipartFormDataContent
        {
            { new StringContent(id!), "Id" }
        };

        var response = await _client.PostAsync("/api/blog-category/delete", deleteForm);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
