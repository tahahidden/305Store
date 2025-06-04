using _305.Tests.Integration.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class BlogBlogControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public BlogBlogControllerTests(CustomWebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetAll_ReturnsOk()
	{
		// Act
		var response = await _client.GetAsync("/api/blog/all");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task CreateCategory_ReturnsSuccess()
	{
		// Arrange
		var form = new MultipartFormDataContent
	{
		{ new StringContent("برنامه‌نویسی"), "Title" },
		{ new StringContent("programming"), "Slug" }
	};

		// Act
		var response = await _client.PostAsync("/api/blog-Blog/create", form);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var json = await response.Content.ReadAsStringAsync();
		json.Should().Match(x => x.Contains("success") || x.Contains("موفق"));
	}
}
