using _305.Application.Features.BlogCategoryFeatures.Command;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace _305.Tests.Integration;

[TestFixture]
public class BlogCategoryControllerTests
{
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Create_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            name = "Integration Category",
            slug = "integration-category",
            description = "Description",
        };

        var content = new MultipartFormDataContent
{
    { new StringContent("Test Category"), "name" },
    { new StringContent("test-category"), "slug" },
};

        var response = await _client.PostAsync("/api/blog-category/create", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(responseBody);
        // Act
        //var response = await _client.PostAsync("/api/blog-category/create", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<_305.Application.Base.Response.ResponseDto<string>>();
        responseData.Should().NotBeNull();
        responseData!.is_success.Should().BeTrue();
    }
}