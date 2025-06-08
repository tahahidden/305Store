using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Tests.Integration.Base.TestController;
using _305.Tests.Unit.DataProvider;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace _305.Tests.Integration.ControllersTests
{
	[TestFixture]
	public class BlogCategoryControllerTests
		: BaseControllerTests<CreateCategoryCommand, string, EditCategoryCommand, BlogCategoryResponse>
	{
		public BlogCategoryControllerTests()
		{
			BaseUrl = "/api/admin/blog-category";
		}

		protected override MultipartFormDataContent CreateCreateForm(CreateCategoryCommand dto)
		{
			return new MultipartFormDataContent
			{
				{ new StringContent(dto.name), "name" },
				{ new StringContent(dto.slug), "slug" }
			};
		}

		protected override MultipartFormDataContent CreateEditForm(EditCategoryCommand dto)
		{
			return new MultipartFormDataContent
			{
				{ new StringContent(dto.id.ToString()), "id" },
				{ new StringContent(dto.name), "name" },
				{ new StringContent(dto.slug), "slug" }
			};
		}

		[Test]
		public async Task Create_Should_Return_Success()
		{
			var createCommand = BlogCategoryDataProvider.Create(name: "new-category");
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

			var response = await Client.PostAsync($"{BaseUrl}/create", form);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public async Task Edit_Should_Return_Success()
		{
			var createCommand = BlogCategoryDataProvider.Create(name: "edit-title", slug: "edit-slug");
			var slug = await CreateEntityAsync(createCommand);
			var category = await GetBySlugOrIdAsync(slug);

			var editCommand = BlogCategoryDataProvider.Edit(name: "edited-title", id: category.id, slug: "edited-slug");
			var editForm = CreateEditForm(editCommand);
			var response = await Client.PostAsync($"{BaseUrl}/edit", editForm);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var editResult = JsonConvert.DeserializeObject<ResponseDto<string>>(json);
			Assert.That(editResult?.is_success, Is.True);
		}

		[Test]
		public async Task GetAll_Should_Return_List()
		{
			var response = await Client.GetAsync($"{BaseUrl}/all");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ResponseDto<List<BlogCategoryResponse>>>(json);

			Assert.That(result?.is_success, Is.True);
			Assert.That(result?.data, Is.Not.Null);
		}

		[Test]
		public async Task Index_Paginated_Should_Return_Success()
		{
			var response = await Client.GetAsync($"{BaseUrl}/list?page=1&pageSize=10");
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task List_Should_Return_Correct_PageSize()
		{
			var response = await Client.GetAsync($"{BaseUrl}/list?page=1&pageSize=5");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var result = await DeserializePaginatedResponse(json);

			Assert.That(result?.is_success, Is.True);
			Assert.That(result!.data.Data.Count, Is.LessThanOrEqualTo(5));
		}

		[Test]
		public async Task GetBySlug_Should_Return_Success()
		{
			var createCommand = BlogCategoryDataProvider.Create(name: "new name", slug: "new-slug");
			await CreateEntityAsync(createCommand);

			var response = await Client.GetAsync($"{BaseUrl}/get?slug=new-slug");
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(json);

			Assert.That(result?.is_success, Is.True);
			Assert.That(result?.data, Is.Not.Null);
		}

		[Test]
		public async Task GetBySlug_Should_Return_NotFound_When_Slug_NotExists()
		{
			var response = await Client.GetAsync($"{BaseUrl}/get?slug=not-exists-slug");
			var json = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ResponseDto<BlogCategoryResponse>>(json);

			Assert.That(result?.is_success, Is.False);
			Assert.That(result?.response_code, Is.EqualTo(404));
		}

		[Test]
		public async Task Delete_Should_Return_Success()
		{
			var createCommand = BlogCategoryDataProvider.Create(name: "edit-title", slug: "edit-slug");
			var slug = await CreateEntityAsync(createCommand);
			var category = await GetBySlugOrIdAsync(slug);

			await DeleteEntityAsync(category.id);
		}
	}
}
