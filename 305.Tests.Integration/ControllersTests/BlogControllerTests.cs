using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Response;
using _305.BuildingBlocks.Helper;
using _305.Tests.Integration.Base.TestController;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace _305.Tests.Integration.ControllersTests;
[TestFixture]
public class BlogControllerTests : BaseControllerTests<CreateBlogCommand, string, EditBlogCommand, BlogResponse>
{
    public BlogControllerTests()
    {
        _baseUrl = "/api/blog";
    }

    protected override MultipartFormDataContent CreateCreateForm(CreateBlogCommand dto)
    {
        var form = new MultipartFormDataContent
    {
        { new StringContent(dto.name), "name" },
        { new StringContent(dto.slug), "slug" }
    };

        if (dto.image_file is not null)
        {
            var stream = dto.image_file.OpenReadStream();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.image_file.ContentType ?? "application/octet-stream");
            form.Add(content, "image_file", dto.image_file.FileName);
        }

        return form;
    }

    protected override MultipartFormDataContent CreateEditForm(EditBlogCommand dto)
    {

        var form = new MultipartFormDataContent
        {
            { new StringContent(dto.id.ToString()), "id" },
            { new StringContent(dto.name), "name" },
            { new StringContent(dto.slug), "slug" }
        };

        if (dto.image_file is not null)
        {
            var stream = dto.image_file.OpenReadStream();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.image_file.ContentType ?? "application/octet-stream");
            form.Add(content, "image_file", dto.image_file.FileName);
        }

        return form;
    }

    [Test]
    public async Task Create_Should_Return_Success()
    {
        var dto = new CreateBlogCommand 
        { name = "test-title",
          slug = "test-title",
          image_file = FakeFileHelper.CreateFakeFormFile("my.jpg", "image/jpeg", "dummy content"),
          blog_category_id = 1,
          description = "",
          estimated_read_time = 2,
          blog_text = "",
          keywords = "a,b,c,d",
          image_alt = "alt",
          meta_description = "meta",
          show_blog = true,
        };
        var key = await CreateEntityAsync(dto);
        Assert.That(key, Is.Not.Null);
    }

    [Test]
    public async Task Create_Should_Return_BadRequest_When_MissingName()
    {
        var form = new MultipartFormDataContent
            {
                { new StringContent("slug-only"), "slug" }
            };

        var response = await _client.PostAsync($"{_baseUrl}/create", form);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Edit_Should_Return_Success()
    {
        var slug = await CreateEntityAsync(new CreateBlogCommand
        {
            name = "test-title",
            slug = "test-title",
            image_file = FakeFileHelper.CreateFakeFormFile("my.jpg", "image/jpeg", "dummy content"),
            blog_category_id = 1,
            description = "",
            estimated_read_time = 2,
            blog_text = "",
            keywords = "a,b,c,d",
            image_alt = "alt",
            meta_description = "meta",
            show_blog = true,
        });
        var category = await GetBySlugOrIdAsync(slug);

        var editForm = CreateEditForm(new EditBlogCommand()
        {
            id = category.id,
            slug = "edited-title",
            name = "edited-slug",
        });
        var response = await _client.PostAsync($"{_baseUrl}/edit", editForm);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var editResult = JsonConvert.DeserializeObject<ResponseDto<string>>(json);
        Assert.That(editResult?.is_success, Is.True);
    }

    [Test]
    public async Task GetAll_Should_Return_List()
    {
        var response = await _client.GetAsync($"{_baseUrl}/all");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<List<BlogResponse>>>(json);

        Assert.That(result?.is_success, Is.True);
        Assert.That(result?.data, Is.Not.Null);
    }

    [Test]
    public async Task Index_Paginated_Should_Return_Success()
    {
        var response = await _client.GetAsync($"{_baseUrl}/list?page=1&pageSize=10");
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task List_Should_Return_Correct_PageSize()
    {
        var response = await _client.GetAsync($"{_baseUrl}/list?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = await DeserializePaginatedResponse(json);

        Assert.That(result?.is_success, Is.True);
        Assert.That(result!.data.Data.Count, Is.LessThanOrEqualTo(5));
    }

    [Test]
    public async Task GetBySlug_Should_Return_Success()
    {
        var dto = new CreateBlogCommand { name = "slug-title", slug = "slug-me" };
        await CreateEntityAsync(dto);

        var response = await _client.GetAsync($"{_baseUrl}/get?slug=slug-me");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<BlogResponse>>(json);

        Assert.That(result?.is_success, Is.True);
        Assert.That(result?.data, Is.Not.Null);
    }

    [Test]
    public async Task GetBySlug_Should_Return_NotFound_When_Slug_NotExists()
    {
        var response = await _client.GetAsync($"{_baseUrl}/get?slug=not-exists-slug");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<BlogResponse>>(json);

        Assert.That(result?.is_success, Is.False);
        Assert.That(result?.response_code, Is.EqualTo(404));
    }

    [Test]
    public async Task Delete_Should_Return_Success()
    {
        var slug = await CreateEntityAsync(new CreateBlogCommand { name = "delete-title", slug = "delete-slug" });
        var category = await GetBySlugOrIdAsync(slug);

        await DeleteEntityAsync(category.id);
    }
}
