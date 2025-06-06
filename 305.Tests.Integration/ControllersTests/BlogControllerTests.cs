using _305.Application.Base.Response;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Domain.Entity;
using _305.Tests.Integration.Base.Helpers;
using _305.Tests.Integration.Base.TestController;
using _305.Tests.Unit.DataProvider;
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
        { new StringContent(dto.slug), "slug" },
        { new StringContent(dto.blog_text), "blog_text" },
        { new StringContent(dto.description), "description" },
        { new StringContent(dto.image_alt), "image_alt" },
        { new StringContent(dto.keywords), "keywords" },
        { new StringContent(dto.meta_description), "meta_description" },
        { new StringContent(dto.blog_category_id.ToString()), "blog_category_id" }

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
            { new StringContent(dto.slug), "slug" },
            { new StringContent(dto.blog_text), "blog_text" },
            { new StringContent(dto.description), "description" },
            { new StringContent(dto.image_alt), "image_alt" },
            { new StringContent(dto.keywords), "keywords" },
            { new StringContent(dto.meta_description), "meta_description" },
            { new StringContent(dto.blog_category_id.ToString()), "blog_category_id" }
        };

        if (dto.image_file is null) return form;
        var stream = dto.image_file.OpenReadStream();
        var content = new StreamContent(stream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.image_file.ContentType ?? "application/octet-stream");
        form.Add(content, "image_file", dto.image_file.FileName);

        return form;
    }

    [Test]
    public async Task Create_Should_Return_Success()
    {
        // Arrange
        var helper = new TestDataHelper(_client);
        var categoryId = await helper.CreateCategoryAndReturnIdAsync();

        var createCommand = BlogDataProvider.Create(name: "new", slug: "new-slug", categoryId: categoryId);
        var slug = await CreateEntityAsync(createCommand);
        Assert.That(slug, Is.Not.Null);
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
        // Arrange
        var helper = new TestDataHelper(_client);
        var categoryId = await helper.CreateCategoryAndReturnIdAsync();

        var createCommand = BlogDataProvider.Create(name: "new", slug: "new-slug", categoryId: categoryId);
        var slug = await CreateEntityAsync(createCommand);
        var entity = await GetBySlugOrIdAsync(slug);

        var editCommand = BlogDataProvider.Edit(id: entity.id, name: "edited-name", categoryId: categoryId);
        var editForm = CreateEditForm(editCommand);
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

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<PaginatedList<Blog>>>(json);

        Assert.That(result?.is_success, Is.True);
        Assert.That(result?.data, Is.Not.Null);
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
        // Arrange
        var helper = new TestDataHelper(_client);
        var categoryId = await helper.CreateCategoryAndReturnIdAsync();

        var createCommand = BlogDataProvider.Create(name: "name", slug: "test-slug", categoryId: categoryId);
        await CreateEntityAsync(createCommand);

        // Act
        var response = await _client.GetAsync($"{_baseUrl}/get?slug=test-slug");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<BlogResponse>>(json);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result?.is_success, Is.True);
            Assert.That(result?.data, Is.Not.Null);
            Assert.That(result?.data.slug, Is.EqualTo("test-slug"));
            Assert.That(result?.data.name, Is.EqualTo("name"));
            Assert.That(result?.data.blog_category_id, Is.EqualTo(categoryId));
        });
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
        // Arrange
        var helper = new TestDataHelper(_client);
        var categoryId = await helper.CreateCategoryAndReturnIdAsync();

        var createCommand = BlogDataProvider.Create(categoryId: categoryId);
        var slug = await CreateEntityAsync(createCommand);
        var category = await GetBySlugOrIdAsync(slug);

        await DeleteEntityAsync(category.id);
    }
}
