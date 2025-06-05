using NUnit.Framework;

namespace _305.Tests.Integration
{
    public abstract class BaseControllerTests<TCreateDto, TResponseDto>
    {
        protected HttpClient _client = null!;
        protected CustomWebApplicationFactory _factory = null!;
        protected string _baseUrl = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        protected abstract MultipartFormDataContent CreateCreateForm(TCreateDto dto);
        protected abstract Task<TResponseDto?> DeserializeResponse(string json);

        /// <summary>
        /// متد عمومی برای ایجاد یک موجودیت جدید و گرفتن کلید اصلی (مثل slug یا id)
        /// </summary>
        public async Task<string> CreateEntityAsync(TCreateDto dto)
        {
            var form = CreateCreateForm(dto);
            var response = await _client.PostAsync($"{_baseUrl}/create", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = await DeserializeResponse(json);

            if (result == null)
                throw new Exception("Create response is null");

            var prop = typeof(TResponseDto).GetProperty("data");
            if (prop == null)
                throw new Exception("ResponseDto has no Data property");
            var key = prop.GetValue(result)?.ToString();
            return key!;
        }

        /// <summary>
        /// متد کمکی برای حذف موجودیت بر اساس id یا کلید اصلی
        /// </summary>
        public async Task DeleteEntityAsync(int id)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "id" }
            };

            var response = await _client.PostAsync($"{_baseUrl}/delete", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = await DeserializeResponse(json);

            var prop = typeof(TResponseDto).GetProperty("is_success") ?? typeof(TResponseDto).GetProperty("is_success");
            if (prop != null)
            {
                var success = prop.GetValue(result);
                Assert.That(success, Is.True);
            }
        }
    }
}
