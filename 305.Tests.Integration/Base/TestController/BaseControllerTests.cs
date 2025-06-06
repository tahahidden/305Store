using _305.Application.Base.Response;
using _305.Application.Filters.Pagination;
using _305.Tests.Integration.Base.Factory;
using _305.Tests.Integration.Base.JWT;
using Newtonsoft.Json;
using NUnit.Framework;

namespace _305.Tests.Integration.Base.TestController;
public abstract class BaseControllerTests<TCreateDto, TKey, TEditDto, TResponse>
{
    protected HttpClient Client = null!;
    protected CustomWebApplicationFactory Factory = null!;
    protected string BaseUrl = null!;
    private JwtTestHelper _jwtHelper = null!;

    [SetUp]
    public void Setup()
    {
        Factory = new CustomWebApplicationFactory();
        Client = Factory.CreateClient();
        _jwtHelper = new JwtTestHelper();
        _jwtHelper.AddTokenToClient(Client, roles: ["Admin"]);
    }

    [TearDown]
    public void TearDown()
    {
        Client.Dispose();
        Factory.Dispose();
    }

    protected abstract MultipartFormDataContent CreateCreateForm(TCreateDto dto);
    protected abstract MultipartFormDataContent CreateEditForm(TEditDto dto);

    protected virtual async Task<ResponseDto<TKey>?> DeserializeCreateResponse(string json)
        => await Task.Run(() => JsonConvert.DeserializeObject<ResponseDto<TKey>>(json));

    protected virtual async Task<ResponseDto<TResponse>?> DeserializeEntityResponse(string json)
        => await Task.Run(() => JsonConvert.DeserializeObject<ResponseDto<TResponse>>(json));

    protected virtual async Task<ResponseDto<PaginatedList<TResponse>>?> DeserializePaginatedResponse(string json)
        => await Task.Run(() => JsonConvert.DeserializeObject<ResponseDto<PaginatedList<TResponse>>>(json));

    public async Task<TKey> CreateEntityAsync(TCreateDto dto)
    {
        var form = CreateCreateForm(dto);
        var response = await Client.PostAsync($"{BaseUrl}/create", form);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = await DeserializeCreateResponse(json);

        Assert.That(result?.is_success, Is.True);
        return result!.data!;
    }

    public async Task<TResponse> GetBySlugOrIdAsync(string slugOrId)
    {
        var response = await Client.GetAsync($"{BaseUrl}/get?slug={slugOrId}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = await DeserializeEntityResponse(json);

        Assert.That(result?.is_success, Is.True);
        return result!.data!;
    }

    public async Task DeleteEntityAsync(long id)
    {
        var form = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "Id" }
            };
        var response = await Client.PostAsync($"{BaseUrl}/delete", form);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = await DeserializeCreateResponse(json);

        Assert.That(result?.is_success, Is.True);
    }

}
