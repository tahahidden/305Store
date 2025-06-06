using _305.Application.IUOW;
using _305.Tests.Integration.Base.Factory;
using _305.WebApi.Assistants.Permission;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace _305.Tests.Integration.Base;
[TestFixture]
public class SyncPermissionsTest
{
	private CustomWebApplicationFactory _factory;
	private IServiceScope _scope;
	private IUnitOfWork _unitOfWork;

	[SetUp]
	public void SetUp()
	{
		_factory = new CustomWebApplicationFactory();
		_scope = _factory.Services.CreateScope();
		_unitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
	}


	[TearDown]
	public void TearDown()
	{
		_scope.Dispose();
		_factory.Dispose();
	}

	[Test]
	public async Task Should_Seed_Permissions_Correctly()
	{
		// Arrange
		var seeder = new PermissionSeeder(_unitOfWork);

		// Act
		await seeder.SyncPermissionsAsync();

		// Assert
		var permissions = _unitOfWork.PermissionRepository.FindList();

		// حداقل باید یکی باشه
		Assert.That(permissions, Is.Not.Empty);

		// به‌صورت اختیاری بررسی دقیق‌تر:
		Assert.That(permissions.Any(p => p.name.Contains("AdminAuth.Login") || p.slug.Contains("AdminAuth.Login")),
			"Expected permission with name or slug containing 'AdminAuth.Login' not found.");
	}
}
