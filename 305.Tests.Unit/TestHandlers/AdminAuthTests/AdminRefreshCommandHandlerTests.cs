using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Handler;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.TestHandlers.AdminAuthTests;
public class AdminRefreshCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNewAccessToken_WhenRefreshTokenIsValid()
    {
        // Arrange
        var user = AdminUserDataProvider.Row();
        var roles = new List<UserRole>
    {
        new UserRole { role = new Role { name = "Admin" } }
    };

        var accessToken = "new_access_token";

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync(user);
        unitOfWorkMock.Setup(u => u.UserRoleRepository.FindList(It.IsAny<Expression<Func<UserRole, bool>>>(), null))
            .Returns(roles);
        unitOfWorkMock.Setup(u => u.TokenBlacklistRepository.ExistsAsync(It.IsAny<Expression<Func<BlacklistedToken, bool>>>()))
            .ReturnsAsync(false);

        var tokenServiceMock = new Mock<IJwtService>();
        tokenServiceMock.Setup(t => t.GenerateAccessToken(user, It.IsAny<List<string>>(), null))
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
        tokenServiceMock.Verify(t => t.GenerateAccessToken(user, It.IsAny<List<string>>(), null), Times.Once);
        unitOfWorkMock.Verify(u => u.TokenBlacklistRepository.ExistsAsync(It.IsAny<Expression<Func<BlacklistedToken, bool>>>()), Times.Once);
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
            new Mock<IJwtService>().Object, httpContextAccessorMock.Object);

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
            refresh_token_expiry_time = DateTime.Now.AddMinutes(-5),// منقضی شده
            mobile = "09333333333",
            concurrency_stamp = "test",
            security_stamp = "test",
            email = "test@305.com"
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
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
            new Mock<IJwtService>().Object, httpContextAccessorMock.Object);

        var result = await handler.Handle(new AdminRefreshCommand(), CancellationToken.None);

        Assert.False(result.is_success);
        Assert.Equal(401, result.response_code);
        responseCookiesMock.Verify(c => c.Delete("jwt"), Times.Once);
    }

}
