using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Tests.Integration;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

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
    public async Task Create_Should_Return_BadRequest_When_MissingName()
    {
        var form = new MultipartFormDataContent
    {
        // حذف فیلد name
        { new StringContent("slug-only"), "slug" }
    };

        var response = await _client.PostAsync("/api/blog-category/create", form);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Edit_Should_Return_Success()
    {
        // ایجاد دسته‌بندی جدید
        var createContent = new MultipartFormDataContent
        {
            { new StringContent("edit-title"), "name" },
            { new StringContent("edit-slug"), "slug" }
        };

        var createResponse = await _client.PostAsync("/api/blog-category/create", createContent);
        createResponse.EnsureSuccessStatusCode();

        var createResultJson = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(createResultJson);

        Assert.That(createResult, Is.Not.Null);
        Assert.That(createResult.is_success, Is.EqualTo(true));
        Assert.That(createResult.data, Is.Not.Null);

        var slug = createResult.data;

        var response = await _client.GetAsync($"/api/blog-category/get?slug={slug}");

        var getResultJson = await response.Content.ReadAsStringAsync();
        var category = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(getResultJson);

        // ویرایش دسته
        var editContent = new MultipartFormDataContent
    {
        { new StringContent(category.data.id.ToString()), "id" },
        { new StringContent("edited-title"), "name" },
        { new StringContent("edited-slug"), "slug" }
    };

        var editResponse = await _client.PostAsync("/api/blog-category/edit", editContent);
        editResponse.EnsureSuccessStatusCode();

        //اگر خواستی می‌تونی محتوای جواب ویرایش رو چاپ کنی برای دیباگ:
        var editResultJson = await editResponse.Content.ReadAsStringAsync();
        var editResult = JsonConvert.DeserializeObject<ResponseDto<string>>(editResultJson);
        Assert.That(editResult, Is.Not.Null);
        Assert.That(editResult.is_success, Is.EqualTo(true));
        Assert.That(editResult.data, Is.Not.Null);
    }



    [Test]
    public async Task GetAll_Should_Return_List()
    {
        var response = await _client.GetAsync("/api/blog-category/all");
        var getResultJson = await response.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<ResponseDto<List<BlogCategoryResponse>>>(getResultJson);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(getResult.is_success, Is.EqualTo(true));
    }

    [Test]
    public async Task Index_Paginated_Should_Return_Success()
    {
        var response = await _client.GetAsync("/api/blog-category/list?page=1&pageSize=10");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task List_Should_Return_Correct_PageSize()
    {
        var response = await _client.GetAsync("/api/blog-category/list?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<PaginatedList<BlogCategoryResponse>>>(content);

        Assert.That(result.is_success, Is.True);
        Assert.That(result.data.Data.Count, Is.LessThanOrEqualTo(5));
    }


    [Test]
    public async Task GetBySlug_Should_Return_Success()
    {
        // ابتدا ایجاد
        var form = new MultipartFormDataContent
        {
            { new StringContent("slug-title"), "name" },
            { new StringContent("slug-me"), "slug" }
        };
        await _client.PostAsync("/api/blog-category/create", form);

        var response = await _client.GetAsync($"/api/blog-category/get?slug=slug-me");

        var getResultJson = await response.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(getResultJson);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(getResult.data, Is.Not.Null);
        Assert.That(getResult.is_success, Is.EqualTo(true));
    }

    [Test]
    public async Task GetBySlug_Should_Return_NotFound_When_Slug_NotExists()
    {
        var response = await _client.GetAsync("/api/blog-category/get?slug=not-exists-slug");

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(content);
        Assert.That(result.is_success, Is.False);
        Assert.That(result.response_code, Is.EqualTo(404));
    }

    [Test]
    public async Task Delete_Should_Return_Success()
    {
        // اول ایجاد
        var form = new MultipartFormDataContent
        {
            { new StringContent("delete-title"), "name" },
            { new StringContent("delete-slug"), "slug" }
        };
        var createResp = await _client.PostAsync("/api/blog-category/create", form);
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(await createResp.Content.ReadAsStringAsync());
        var slug = createResult?.data;

        var getResponse = await _client.GetAsync($"/api/blog-category/get?slug={slug}");
        var getResultJson = await getResponse.Content.ReadAsStringAsync();
        var category = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(getResultJson);

        // حالا حذف
        var deleteForm = new MultipartFormDataContent
        {
            { new StringContent(category.data.id.ToString()), "Id" }
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
