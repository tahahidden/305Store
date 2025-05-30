using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using _305.Tests.Integration.Base;
using _305.Application.IUOW;
using NUnit.Framework;
using _305.Infrastructure.UnitOfWork;
using _305.Application.Features.BlogCategoryFeatures.Command;
using System.Net.Http.Json;
using FluentAssertions;

namespace _305.Tests.Integration.APITest;
public class BlogCategoryAPITests : ControllerBaseTests
{
	private string url = "/api/blog-category";

	private IUnitOfWork _unitOfWork;

	[OneTimeSetUp]
	public void OneTimeSetup()
	{
		_unitOfWork = new UnitOfWork(GetContext());
	}

	[SetUp]
	public void SetUp()
	{
		StartDatabase();
		ResetDatabase();
	}

	[Theory]
	[TestCase("Robert C. Martin", HttpStatusCode.OK, 1)]
	[TestCase(null, HttpStatusCode.BadRequest, 0)]
	public void Must_Add_Valid_Author(string? authorName, HttpStatusCode httpStatusCode, int count)
	{
		//Arrange
		var formData = new MultipartFormDataContent();
		formData.Add(new StringContent(authorName), "name");
		formData.Add(new StringContent(authorName), "description");

		var httpResponseMessage = _httpClient.PostAsync(url + "/create", formData).Result;

		//Assert
		httpResponseMessage.StatusCode.Should().Be(httpStatusCode);
		_unitOfWork.BlogCategoryRepository.FindList().Count().Should().Be(count);
	}

	//[Test]
	//public void Must_Update_Valid_Author()
	//{
	//    //Arrange
	//    const string authorName = "Eric Evans";
	//    var addedAuthor = new Author("Martin Fowler");
	//    base.SeedData(addedAuthor);
	//    var author = new UpdateAuthorCommand { Id = addedAuthor.Id, Name = authorName };

	//    //Act
	//    var httpResponseMessage = _httpClient.PutAsJsonAsync(url, author).Result;

	//    //Assert
	//    httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
	//    var actual = (_authorRepository.GetAllAsync().Result).Single(a => a.Id == author.Id);
	//    actual.Name.Should().Be(authorName);
	//}

	//[Test]
	//public void Must_Not_Update_Invalid_Author()
	//{
	//    //Arrange
	//    var author = new UpdateAuthorCommand { Id = Guid.Empty, Name = "Eric Evans" };

	//    //Act
	//    var httpResponseMessage = _httpClient.PutAsJsonAsync(url, author).Result;

	//    //Assert
	//    httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	//}
}
