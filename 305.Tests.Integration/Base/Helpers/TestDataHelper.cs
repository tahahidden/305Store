using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Features.RoleFeatures.Response;
using Newtonsoft.Json;

namespace _305.Tests.Integration.Base.Helpers;

public class TestDataHelper
{
    private readonly HttpClient _client;

    public TestDataHelper(HttpClient client)
    {
        _client = client;
    }

    private async Task<long> CreateAndGetIdAsync<TResponse>(string createUrl,
        MultipartFormDataContent content, string getUrlPrefix)
        where TResponse : class
    {
        var response = await _client.PostAsync(createUrl, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(json)!;

        var getResponse = await _client.GetAsync($"{getUrlPrefix}{createResult.data}");
        var getJson = await getResponse.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<ResponseDto<TResponse>>(getJson)!;

        return getResult.data.id;
    }

    public Task<long> CreateCategoryAndReturnIdAsync()
    {
        var categoryDto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" }
        };

        return CreateAndGetIdAsync<BlogCategoryResponse>(
            "/api/admin/blog-category/create", categoryDto,
            "/api/admin/blog-category/get?slug=");
    }

    public Task<long> CreateUserAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
            { new StringContent("password"), "password" },
            { new StringContent($"email@{Guid.NewGuid()}.com"), "email" },
        };

        return CreateAndGetIdAsync<UserResponse>(
            "/api/admin/user/create", dto,
            "/api/admin/user/get?slug=");
    }

    public Task<long> CreateRoleAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
        };

        return CreateAndGetIdAsync<RoleResponse>(
            "/api/admin/role/create", dto,
            "/api/admin/role/get?slug=");
    }

    public Task<long> CreatePermissionAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
        };

        return CreateAndGetIdAsync<UserResponse>(
            "/api/admin/permission/create", dto,
            "/api/admin/permission/get?slug=");
    }
    // سایر موجودیت‌ها (مثلاً CreateBlogAndReturnIdAsync) هم همین‌جا اضافه می‌کنی
}
