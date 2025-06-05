using _305.Domain.Entity;
using _305.Tests.Integration.Base.Factory;
using _305.WebApi.Assistants.Permission;
using Moq;
using NUnit.Framework;

namespace _305.Tests.Integration.Base;
public class SyncPermissionsTest
{
	[Test]
	public async Task SyncPermissions_Should_AddNewPermissions_AndAssignToMainAdmin()
	{
		// Arrange
		var (fakeUow, permissionMock, rolePermissionMock) = UnitOfWorkMockFactory.CreateFakeUnitOfWork();
		var seeder = new PermissionSeeder(fakeUow);

		// Act
		await seeder.SyncPermissionsAsync();

		// Assert
		permissionMock.Verify(p => p.AddAsync(It.IsAny<Permission>()), Times.AtLeastOnce);
		rolePermissionMock.Verify(rp => rp.AddAsync(It.IsAny<RolePermission>()), Times.AtLeastOnce);
	}
}
