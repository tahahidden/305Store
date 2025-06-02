using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Handler;
using _305.Application.IRepository;
using _305.Application.IService;
using _305.Application.IUOW;
using _305.BuildingBlocks.Security;
using _305.Domain.Entity;

namespace _305.Tests.Unit.TestHandlers.AdminAuthTests;
public class AdminLoginCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldLoginSuccessfully_AndSetCookie()
	{
		// Arrange
		var user = new User
		{
			id = 1,
			email = "admin@example.com",
			password_hash = PasswordHasher.Hash("correctpassword"),
			is_active = true,
			is_locked_out = false,
			failed_login_count = 0,
			mobile = "0933333333",
			concurrency_stamp = "test",
			security_stamp = "test"
		};

		var userRoles = new List<UserRole>
	{
		new UserRole { role = new Role { name = "Admin" } }
	};

		var token = "access_token";
		var refreshToken = "refresh_token";

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync(user);
		unitOfWorkMock.Setup(u => u.UserRoleRepository.FindList(It.IsAny<Expression<Func<UserRole, bool>>>(), null))
			.Returns(userRoles);
		unitOfWorkMock.Setup(u => u.TokenBlacklistRepository.ExistsAsync(It.IsAny<Expression<Func<BlacklistedToken, bool>>>()))
			.ReturnsAsync(false);
		unitOfWorkMock.Setup(u => u.UserRepository.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
			.ReturnsAsync(false);

		var tokenServiceMock = new Mock<IJwtService>();
		tokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<User>(), It.IsAny<List<string>>(), null))
			.Returns(token);
		tokenServiceMock.Setup(t => t.GenerateRefreshToken())
			.Returns(refreshToken);

		// Mock Cookies
		var responseCookiesMock = new Mock<IResponseCookies>();
		responseCookiesMock.Setup(c => c.Append(
			It.IsAny<string>(),
			It.IsAny<string>(),
			It.IsAny<CookieOptions>()));

		// Mock Response
		var responseMock = new Mock<HttpResponse>();
		responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);

		// Mock HttpContext
		var httpContextMock = new Mock<HttpContext>();
		httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);

		// Mock IHttpContextAccessor
		var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
		httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);

		// Handler
		var handler = new AdminLoginCommandHandler(unitOfWorkMock.Object, tokenServiceMock.Object, httpContextAccessorMock.Object);

		var command = new AdminLoginCommand
		{
			email = "admin@example.com",
			password = "correctpassword"
		};

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.True(result.is_success);
		Assert.Equal(token, result.data.access_token);
		responseCookiesMock.Verify(c => c.Append("jwt", refreshToken, It.IsAny<CookieOptions>()), Times.Once);
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}


	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenUserNotExistsOrInactive()
	{
		// Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync((User)null);

		var handler = new AdminLoginCommandHandler(unitOfWorkMock.Object, Mock.Of<IJwtService>(), Mock.Of<IHttpContextAccessor>());

		var command = new AdminLoginCommand { email = "notfound@example.com", password = "password" };

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.False(result.is_success);
		Assert.Equal("کاربر پیدا نشد", result.message);
	}


	[Fact]
	public async Task Handle_ShouldReturnLockedOut_WhenUserIsLocked()
	{
		// Arrange
		var user = new User
		{
			email = "test@example.com",
			password_hash = PasswordHasher.Hash("pass"),
			is_active = true,
			is_locked_out = true,
			lock_out_end_time = DateTime.Now.AddMinutes(5),
			mobile = "09333333333",
			concurrency_stamp = "test",
			security_stamp = "test"
		};

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync(user);

		var handler = new AdminLoginCommandHandler(unitOfWorkMock.Object, Mock.Of<IJwtService>(), Mock.Of<IHttpContextAccessor>());

		// Act
		var result = await handler.Handle(new AdminLoginCommand { email = "test@example.com", password = "pass" }, CancellationToken.None);

		// Assert
		Assert.False(result.is_success);
		Assert.Equal(401, result.response_code);
		Assert.Equal("اکانت شما موقتا قفل شده است", result.message);
	}


	[Fact]
	public async Task Handle_ShouldIncreaseFailedLoginCount_WhenPasswordIsWrong()
	{
		// Arrange
		var user = new User
		{
			email = "admin@example.com",
			password_hash = PasswordHasher.Hash("correctpassword"),
			is_active = true,
			is_locked_out = false,
			failed_login_count = 3,
			mobile = "09333333333",
			concurrency_stamp = "test",
			security_stamp = "test"
		};

		var userRepoMock = new Mock<IUserRepository>();
		userRepoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync(user);

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository).Returns(userRepoMock.Object);

		var handler = new AdminLoginCommandHandler(unitOfWorkMock.Object, Mock.Of<IJwtService>(), Mock.Of<IHttpContextAccessor>());

		var command = new AdminLoginCommand { email = "admin@example.com", password = "wrongpassword" };

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.False(result.is_success);
		Assert.Equal(401, result.response_code);
		Assert.Equal("رمز عبور یا نام کاربری اشتباه است.", result.message);
		Assert.Equal(4, user.failed_login_count);
		unitOfWorkMock.Verify(u => u.UserRepository.Update(It.IsAny<User>()), Times.Once);
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}


	[Fact]
	public async Task Handle_ShouldLockUser_WhenFailedLoginCountReachesThreshold()
	{
		// Arrange
		var user = new User
		{
			email = "admin@example.com",
			password_hash = PasswordHasher.Hash("correctpassword"),
			is_active = true,
			is_locked_out = false,
			failed_login_count = 4,
			mobile = "09333333333",
			concurrency_stamp = "test",
			security_stamp = "test"
		};

		var userRepoMock = new Mock<IUserRepository>();
		userRepoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<User, bool>>>(), null))
			.ReturnsAsync(user);

		var unitOfWorkMock = new Mock<IUnitOfWork>();
		unitOfWorkMock.Setup(u => u.UserRepository).Returns(userRepoMock.Object);

		var handler = new AdminLoginCommandHandler(unitOfWorkMock.Object, Mock.Of<IJwtService>(), Mock.Of<IHttpContextAccessor>());

		var command = new AdminLoginCommand { email = "admin@example.com", password = "wrongpassword" };

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.False(result.is_success);
		Assert.True(user.is_locked_out);
		Assert.True(user.lock_out_end_time > DateTime.Now);
		unitOfWorkMock.Verify(u => u.UserRepository.Update(It.IsAny<User>()), Times.Once);
		unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}


}
