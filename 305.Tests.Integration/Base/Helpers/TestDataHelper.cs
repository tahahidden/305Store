using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Features.RoleFeatures.Response;
using Newtonsoft.Json;

namespace _305.Tests.Integration.Base.Helpers;

public class TestDataHelper(HttpClient client)
{
	public async Task<long> CreateCategoryAndReturnIdAsync()
	{
		var categoryDto = new MultipartFormDataContent
			{
				{ new StringContent("name"), "name" },
				{ new StringContent("slug"), "slug" }
			};

		var response = await client.PostAsync("/api/admin/blog-category/create", categoryDto);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<string>>(json);

		var getResponse = await client.GetAsync($"/api/admin/blog-category/get?slug={result.data}");

		var getJson = await getResponse.Content.ReadAsStringAsync();
		var getResult = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(getJson);

		return getResult.data.id;
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

		var response = await client.PostAsync("/api/admin/user/create", dto);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<string>>(json);

		var getResponse = await client.GetAsync($"/api/admin/user/get?slug={result.data}");

		var getJson = await getResponse.Content.ReadAsStringAsync();
		var getResult = JsonConvert.DeserializeObject<ResponseDto<UserResponse>>(getJson);

		return getResult.data.id;
	}

	public async Task<long> CreateRoleAndReturnIdAsync()
	{
		var dto = new MultipartFormDataContent
		{
			{ new StringContent("name"), "name" },
			{ new StringContent("slug"), "slug" },
			
		};

		var response = await client.PostAsync("/api/admin/role/create", dto);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<string>>(json);

		var getResponse = await client.GetAsync($"/api/admin/role/get?slug={result.data}");

		var getJson = await getResponse.Content.ReadAsStringAsync();
		var getResult = JsonConvert.DeserializeObject<ResponseDto<RoleResponse>>(getJson);

		return getResult.data.id;
	}

	public async Task<long> CreatePermissionAndReturnIdAsync()
	{
		var dto = new MultipartFormDataContent
		{
			{ new StringContent("name"), "name" },
			{ new StringContent("slug"), "slug" },
		};

		var response = await client.PostAsync("/api/admin/permission/create", dto);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<string>>(json);

		var getResponse = await client.GetAsync($"/api/admin/permission/get?slug={result.data}");

		var getJson = await getResponse.Content.ReadAsStringAsync();
		var getResult = JsonConvert.DeserializeObject<ResponseDto<UserResponse>>(getJson);

		return getResult.data.id;
	}
	// سایر موجودیت‌ها (مثلاً CreateBlogAndReturnIdAsync) هم همین‌جا اضافه می‌کنی
}
