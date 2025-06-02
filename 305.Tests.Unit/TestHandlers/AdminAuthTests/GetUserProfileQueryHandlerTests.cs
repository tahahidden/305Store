using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Security.Claims;
using _305.Application.Features.AdminAuthFeatures.Handler;
using _305.Application.Features.AdminAuthFeatures.Query;
using _305.Application.IUOW;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;

namespace _305.Tests.Unit.TestHandlers.AdminAuthTests;
public class GetUserProfileQueryHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnUserProfile_WhenUserExists()
	{
		// Arrange
		const string userId = "1";
		var user = AdminUserDataProvider.Row(id:int.Parse(userId));

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(x => x.UserRepository.FindSingleAsNoTracking(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync(user);

		var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
		var identity = new ClaimsIdentity(claims);
		var principal = new ClaimsPrincipal(identity);

		var httpContext = new DefaultHttpContext { User = principal };
		var httpContextAccessor = new Mock<IHttpContextAccessor>();
		httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

		var handler = new GetUserProfileQueryHandler(unitOfWorkMock.Object, httpContextAccessor.Object);

		// Act
		var result = await handler.Handle(new GetUserProfileQuery(), CancellationToken.None);

		// Assert
		Assert.True(result.is_success);
		Assert.NotNull(result.data);
		Assert.Equal(user.name, result.data.name);
	}

	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenUserNotExists()
	{
		const string userId = "1";

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(x => x.UserRepository.FindSingleAsNoTracking(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync((User)null!);

		var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
		var identity = new ClaimsIdentity(claims);
		var principal = new ClaimsPrincipal(identity);

		var httpContext = new DefaultHttpContext { User = principal };
		var httpContextAccessor = new Mock<IHttpContextAccessor>();
		httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

		var handler = new GetUserProfileQueryHandler(unitOfWorkMock.Object, httpContextAccessor.Object);

		var result = await handler.Handle(new GetUserProfileQuery(), CancellationToken.None);

		Assert.False(result.is_success);
	}


	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenUserIdClaimMissing()
	{
		var httpContext = new DefaultHttpContext(); // بدون کاربر
		var httpContextAccessor = new Mock<IHttpContextAccessor>();
		httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

		var handler = new GetUserProfileQueryHandler(Mock.Of<IUnitOfWork>(), httpContextAccessor.Object);

		var result = await handler.Handle(new GetUserProfileQuery(), CancellationToken.None);

		Assert.False(result.is_success);
	}

	[Fact]
	public async Task Handle_ShouldReturnExceptionFail_WhenUnhandledExceptionOccurs()
	{
		const string userId = "1";

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(x => x.UserRepository.FindSingleAsNoTracking(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ThrowsAsync(new Exception("Database failure"));

		var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
		var identity = new ClaimsIdentity(claims);
		var principal = new ClaimsPrincipal(identity);

		var httpContext = new DefaultHttpContext { User = principal };
		var httpContextAccessor = new Mock<IHttpContextAccessor>();
		httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

		var handler = new GetUserProfileQueryHandler(unitOfWorkMock.Object, httpContextAccessor.Object);

		var result = await handler.Handle(new GetUserProfileQuery(), CancellationToken.None);

		Assert.False(result.is_success);
		Assert.Equal(500, result.response_code);
	}

}
