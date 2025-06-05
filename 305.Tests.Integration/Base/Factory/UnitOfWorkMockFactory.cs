using _305.Application.IBaseRepository;
using _305.Application.IRepository;
using _305.Application.IUOW;
using _305.Domain.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _305.Tests.Integration.Base.Factory;
public static class UnitOfWorkMockFactory
{
	public static (IUnitOfWork Uow, Mock<IPermissionRepository> PermissionRepoMock, Mock<IRolePermissionRepository> RolePermissionRepoMock) CreateFakeUnitOfWork()
	{
		var uowMock = new Mock<IUnitOfWork>();

		// ریپازیتوری‌ها
		var permissionRepoMock = new Mock<IPermissionRepository>();
		var roleRepoMock = new Mock<IRoleRepository>();
		var rolePermissionRepoMock = new Mock<IRolePermissionRepository>();

		// دیتا ساختگی
		var existingPermissions = new List<Permission>
		{
			new() { id = 1, name = "User.Create", slug = "User.Create", created_at = DateTime.Now }
		};

		permissionRepoMock.Setup(r => r.FindList(null)).Returns(existingPermissions);
		permissionRepoMock.Setup(r => r.AddAsync(It.IsAny<Permission>())).Returns(Task.CompletedTask);

		roleRepoMock.Setup(r => r.FindSingle(It.IsAny<Expression<Func<Role, bool>>>(), null))
			.ReturnsAsync(new Role { id = 1, name = "MainAdmin" });

		rolePermissionRepoMock.Setup(r => r.FindList(null)).Returns([
			new RolePermission { slug = "User.CreateMainAdmin" }
		]);
		rolePermissionRepoMock.Setup(r => r.AddAsync(It.IsAny<RolePermission>())).Returns(Task.CompletedTask);

		// اتصال به UOW
		uowMock.SetupGet(u => u.PermissionRepository).Returns(permissionRepoMock.Object);
		uowMock.SetupGet(u => u.RoleRepository).Returns(roleRepoMock.Object);
		uowMock.SetupGet(u => u.RolePermissionRepository).Returns(rolePermissionRepoMock.Object);
		uowMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

		return (uowMock.Object, permissionRepoMock, rolePermissionRepoMock);
	}
}