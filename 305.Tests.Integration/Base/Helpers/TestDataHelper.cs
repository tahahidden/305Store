using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Features.RoleFeatures.Response;
using Newtonsoft.Json;

namespace _305.Tests.Integration.Base.Helpers;

public class TestDataHelper(HttpClient client)
{
	private async Task<TResponse?> CreateAndGetIdAsync<TResponse>(string createUrl,
        MultipartFormDataContent content, string getUrlPrefix)
        where TResponse : class
    {
        var response = await client.PostAsync(createUrl, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<ResponseDto<string>>(json)!;

        var getResponse = await client.GetAsync($"{getUrlPrefix}{createResult.data}");
        var getJson = await getResponse.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<ResponseDto<TResponse>>(getJson)!;

        return getResult.data;
    }

    public async Task<long> CreateCategoryAndReturnIdAsync()
    {
        var categoryDto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" }
        };

        return (await CreateAndGetIdAsync<BlogCategoryResponse>(
            $"{BaseUrlProvider.AdminApi}blog-category/create", categoryDto,
            $"{BaseUrlProvider.AdminApi}blog-category/get?slug="))?.id ?? 0;
    }

    public async Task<long> CreateUserAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
            { new StringContent("password"), "password" },
            { new StringContent($"email@{Guid.NewGuid()}.com"), "email" },
        };

        return ( await CreateAndGetIdAsync<UserResponse>(
            $"{BaseUrlProvider.AdminApi}user/create", dto,
            $"{BaseUrlProvider.AdminApi}user/get?slug="))?.id ?? 0;
    }

    public async Task<long> CreateRoleAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
        };

        return (await CreateAndGetIdAsync<RoleResponse>(
            $"{BaseUrlProvider.AdminApi}role/create", dto,
            $"{BaseUrlProvider.AdminApi}role/get?slug="))?.id ?? 0;
    }

    public async Task<long> CreatePermissionAndReturnIdAsync()
    {
        var dto = new MultipartFormDataContent
        {
            { new StringContent("name"), "name" },
            { new StringContent("slug"), "slug" },
        };

        return (await CreateAndGetIdAsync<UserResponse>(
            $"{BaseUrlProvider.AdminApi}permission/create", dto,
            $"{BaseUrlProvider.AdminApi}permission/get?slug="))?.id ?? 0;
    }
    // سایر موجودیت‌ها (مثلاً CreateBlogAndReturnIdAsync) هم همین‌جا اضافه می‌کنی
}
