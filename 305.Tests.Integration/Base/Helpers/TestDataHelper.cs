using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Response;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Json;

namespace _305.Tests.Integration.Base.Helpers;

public class TestDataHelper
{
    private readonly HttpClient _client;

    public TestDataHelper(HttpClient client)
    {
        _client = client;
    }

    public async Task<long> CreateCategoryAndReturnIdAsync()
    {
        var categoryDto = new MultipartFormDataContent
            {
                { new StringContent("name"), "name" },
                { new StringContent("slug"), "slug" }
            };

        var response = await _client.PostAsync("/api/blog-category/create", categoryDto);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseDto<string>>(json);

        var getResponse = await _client.GetAsync($"/api/blog-category/get?slug={result.data}");

        var getJson = await getResponse.Content.ReadAsStringAsync();
        var getResult = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(getJson);

        return getResult.data.id;
    }

    // سایر موجودیت‌ها (مثلاً CreateBlogAndReturnIdAsync) هم همین‌جا اضافه می‌کنی
}
