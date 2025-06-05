using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Response;
using _305.Application.Filters.Pagination;
using _305.Tests.Integration;
using _305.Tests.Integration.Base.DTOs;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace _305.IntegrationTests.Controllers
{
    [TestFixture]
    public class BlogCategoryControllerTests : BaseControllerTests<CreateBlogCategoryDto, TestResponseDto<string>>
    {
        public BlogCategoryControllerTests()
        {
            _baseUrl = "/api/blog-category";
        }

        protected override MultipartFormDataContent CreateCreateForm(CreateBlogCategoryDto dto)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(dto.name), "name" },
                { new StringContent(dto.slug), "slug" }
            };
            return form;
        }

        protected MultipartFormDataContent CreateEditForm(BlogCategoryResponse dto)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(dto.id.ToString()), "id" },
                { new StringContent(dto.name), "name" },
                { new StringContent(dto.slug), "slug" }
            };
            return form;
        }

        protected override async Task<TestResponseDto<string>?> DeserializeResponse(string json)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<TestResponseDto<string>>(json));
        }

        protected async Task<TestResponseDto<BlogCategoryResponse>?> DeserializeDetailResponse(string json)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<TestResponseDto<BlogCategoryResponse>>(json));
        }

        protected async Task<TestResponseDto<PaginatedList<BlogCategoryResponse>>?> DeserializePaginatedResponse(string json)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<TestResponseDto<PaginatedList<BlogCategoryResponse>>>(json));
        }

        /// <summary>
        /// دریافت داده از api بر اساس slug یا id
        /// </summary>
        private async Task<BlogCategoryResponse> GetBySlugOrIdAsync(string slugOrId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/get?slug={slugOrId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = await DeserializeDetailResponse(json);

            if (result == null || result.data == null)
                throw new Exception("Entity not found");

            return result.data;
        }

        /// <summary>
        /// حذف موجودیت از طریق api
        /// </summary>
        private async Task DeleteEntityAsync(long id)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "Id" }
            };
            var response = await _client.PostAsync($"{_baseUrl}/delete", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = await DeserializeResponse(json);

            Assert.That(result?.is_success ?? result?.is_success, Is.True);
        }

        [Test]
        public async Task Create_Should_Return_Success()
        {
            var dto = new CreateBlogCategoryDto { name = "test-title", slug = "test-title" };
            var key = await CreateEntityAsync(dto);
            Assert.That(key, Is.Not.Null);
        }

        [Test]
        public async Task Create_Should_Return_BadRequest_When_MissingName()
        {
            var form = new MultipartFormDataContent
            {
                // حذف فیلد name عمداً برای تست خطا
                { new StringContent("slug-only"), "slug" }
            };

            var response = await _client.PostAsync($"{_baseUrl}/create", form);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Edit_Should_Return_Success()
        {
            var slug = await CreateEntityAsync(new CreateBlogCategoryDto { name = "edit-title", slug = "edit-slug" });
            var category = await GetBySlugOrIdAsync(slug);

            category.name = "edited-title";
            category.slug = "edited-slug";

            var editForm = CreateEditForm(category);
            var response = await _client.PostAsync($"{_baseUrl}/edit", editForm);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var editResult = JsonConvert.DeserializeObject<TestResponseDto<string>>(json);
            Assert.That(editResult?.is_success ?? editResult?.is_success, Is.True);
            Assert.That(editResult?.data ?? editResult?.data, Is.Not.Null);
        }

        [Test]
        public async Task GetAll_Should_Return_List()
        {
            var response = await _client.GetAsync($"{_baseUrl}/all");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TestResponseDto<List<BlogCategoryResponse>>>(json);

            Assert.That(result?.is_success ?? result?.is_success, Is.True);
            Assert.That(result?.data ?? result?.data, Is.Not.Null);
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

            Assert.That(result?.is_success ?? result?.is_success, Is.True);
            Assert.That(result?.data.Data.Count, Is.LessThanOrEqualTo(5));
        }

        [Test]
        public async Task GetBySlug_Should_Return_Success()
        {
            var dto = new CreateBlogCategoryDto { name = "slug-title", slug = "slug-me" };
            await CreateEntityAsync(dto);

            var response = await _client.GetAsync($"{_baseUrl}/get?slug=slug-me");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TestResponseDto<BlogCategoryResponse>>(json);

            Assert.That(result?.is_success ?? result?.is_success, Is.True);
            Assert.That(result?.data ?? result?.data, Is.Not.Null);
        }

        [Test]
        public async Task GetBySlug_Should_Return_NotFound_When_Slug_NotExists()
        {
            var response = await _client.GetAsync($"{_baseUrl}/get?slug=not-exists-slug");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TestResponseDto<BlogCategoryResponse>>(json);

            Assert.That(result?.is_success ?? result?.is_success, Is.False);
            Assert.That(result?.response_code ?? result?.response_code, Is.EqualTo(404));
        }

        [Test]
        public async Task Delete_Should_Return_Success()
        {
            var slug = await CreateEntityAsync(new CreateBlogCategoryDto { name = "delete-title", slug = "delete-slug" });
            var category = await GetBySlugOrIdAsync(slug);

            await DeleteEntityAsync(category.id);
        }
    }
}
