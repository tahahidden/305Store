using Core.EntityFramework.Models;
using DataLayer.Repository;
using GoldAPI.Application.AdminAuthFeatures.Command;
using GoldAPI.Application.AdminAuthFeatures.Handler;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GoldAPI.Test.TestHandlers.AdminAuthTests;
public class AdminRefreshCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnNewAccessToken_WhenRefreshTokenIsValid()
	{
		// Arrange
		var user = new User
		{
			id = 1,
			refresh_token = "valid_refresh_token",
			refresh_token_expiry_time = DateTime.Now.AddMinutes(10)
		};
		var roles = new List<UserRole>
	{
		new UserRole { role = new Role { title = "Admin" } }
	};

		var accessToken = "new_access_token";

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>()))
			.ReturnsAsync(user);
		unitOfWorkMock.Setup(u => u.UserRoleRepository.FindList(It.IsAny<Expression<Func<UserRole, bool>>>()))
			.Returns(roles);
		unitOfWorkMock.Setup(u => u.TokenBlacklistRepository.ExistsAsync(It.IsAny<Expression<Func<BlacklistedToken, bool>>>()))
			.ReturnsAsync(false);

		var tokenServiceMock = new Mock<IJwtTokenService>();
		tokenServiceMock.Setup(t => t.GenerateToken(user, It.IsAny<List<string>>()))
			.Returns(accessToken);

		var requestCookiesMock = new Mock<IRequestCookieCollection>();
		requestCookiesMock.Setup(c => c.TryGetValue("jwt", out It.Ref<string>.IsAny))
			.Returns((string key, out string value) => { value = "valid_refresh_token"; return true; });

		var responseCookiesMock = new Mock<IResponseCookies>();

		var responseMock = new Mock<HttpResponse>();
		responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);

		var requestMock = new Mock<HttpRequest>();
		requestMock.Setup(r => r.Cookies).Returns(requestCookiesMock.Object);

		var contextMock = new Mock<HttpContext>();
		contextMock.Setup(c => c.Request).Returns(requestMock.Object);
		contextMock.Setup(c => c.Response).Returns(responseMock.Object);

		var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
		httpContextAccessorMock.Setup(h => h.HttpContext).Returns(contextMock.Object);

		var handler = new AdminRefreshCommandHandler(unitOfWorkMock.Object, tokenServiceMock.Object, httpContextAccessorMock.Object);

		// Act
		var result = await handler.Handle(new AdminRefreshCommand(), CancellationToken.None);

		// Assert
		Assert.True(result.is_success);
		Assert.Equal(accessToken, result.data.access_token);
	}

	[Fact]
	public async Task Handle_ShouldFail_WhenNoRefreshTokenInCookie()
	{
		var requestCookiesMock = new Mock<IRequestCookieCollection>();
		requestCookiesMock.Setup(c => c.TryGetValue("jwt", out It.Ref<string>.IsAny))
			.Returns(false);

		var requestMock = new Mock<HttpRequest>();
		requestMock.Setup(r => r.Cookies).Returns(requestCookiesMock.Object);

		var responseMock = new Mock<HttpResponse>();
		var contextMock = new Mock<HttpContext>();
		contextMock.Setup(c => c.Request).Returns(requestMock.Object);
		contextMock.Setup(c => c.Response).Returns(responseMock.Object);

		var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
		httpContextAccessorMock.Setup(h => h.HttpContext).Returns(contextMock.Object);

		var handler = new AdminRefreshCommandHandler(new Mock<IUnitOfWork>().Object,
			new Mock<IJwtTokenService>().Object, httpContextAccessorMock.Object);

		var result = await handler.Handle(new AdminRefreshCommand(), CancellationToken.None);

		Assert.False(result.is_success);
		Assert.Equal(401, result.response_code);
	}


	[Fact]
	public async Task Handle_ShouldFail_WhenRefreshTokenIsInvalidOrExpired()
	{
		var user = new User
		{
			refresh_token = "expired_token",
			refresh_token_expiry_time = DateTime.Now.AddMinutes(-5) // منقضی شده
		};

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>()))
			.ReturnsAsync(user);

		var requestCookiesMock = new Mock<IRequestCookieCollection>();
		requestCookiesMock.Setup(c => c.TryGetValue("jwt", out It.Ref<string>.IsAny))
			.Returns((string key, out string value) => { value = "expired_token"; return true; });

		var responseCookiesMock = new Mock<IResponseCookies>();
		responseCookiesMock.Setup(c => c.Delete("jwt"));

		var requestMock = new Mock<HttpRequest>();
		requestMock.Setup(r => r.Cookies).Returns(requestCookiesMock.Object);

		var responseMock = new Mock<HttpResponse>();
		responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);

		var contextMock = new Mock<HttpContext>();
		contextMock.Setup(c => c.Request).Returns(requestMock.Object);
		contextMock.Setup(c => c.Response).Returns(responseMock.Object);

		var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
		httpContextAccessorMock.Setup(h => h.HttpContext).Returns(contextMock.Object);

		var handler = new AdminRefreshCommandHandler(unitOfWorkMock.Object,
			new Mock<IJwtTokenService>().Object, httpContextAccessorMock.Object);

		var result = await handler.Handle(new AdminRefreshCommand(), CancellationToken.None);

		Assert.False(result.is_success);
		Assert.Equal(401, result.response_code);
		responseCookiesMock.Verify(c => c.Delete("jwt"), Times.Once);
	}

}
