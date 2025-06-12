using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.RolePermissionTests;
public class EditRolePermissionCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldEditRolePermission_WhenEntityExists()
	{
		var roleRepoMock = new Mock<IRepository<Role>>();
		roleRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
			.ReturnsAsync(true);
		var permissionRepoMock = new Mock<IRepository<Permission>>();
		permissionRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
			.ReturnsAsync(true);

		await EditHandlerTestHelper.TestEditSuccess(
			handlerFactory: (repo, uow) => new EditRolePermissionCommandHandler(uow, repo), 
			execute: (handler, command, token) => handler.Handle(command, token),
			command: RolePermissionDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,
			existingEntity: RolePermissionDataProvider.Row(name: "old", id: 1),
			assertUpdated: entity =>
			{
				Assert.Equal("Updated Name", entity.name);
			},
			setupUowMock: uow =>
			{
				uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
				uow.Setup(x => x.PermissionRepository).Returns(permissionRepoMock.Object);
			}
		);
	}


	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
	{
		await EditHandlerTestHelper.TestEditNotFound<EditRolePermissionCommand, RolePermission, EditRolePermissionCommandHandler>(
			handlerFactory: (repo, uow) => new EditRolePermissionCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: RolePermissionDataProvider.Edit(id: 2),
			entityId: 2
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnCommitFail_WhenCommitFails()
	{
		var roleRepoMock = new Mock<IRepository<Role>>();
		roleRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
			.ReturnsAsync(true);
		var permissionRepoMock = new Mock<IRepository<Permission>>();
		permissionRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Permission, bool>>>()))
			.ReturnsAsync(true);
		await EditHandlerTestHelper.TestEditCommitFail(
			handlerFactory: (repo, uow) => new EditRolePermissionCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: RolePermissionDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,
			existingEntity: RolePermissionDataProvider.Row(name: "old Name", id: 1),
			setupUowMock: uow =>
			{
				uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
				uow.Setup(x => x.PermissionRepository).Returns(permissionRepoMock.Object);
			}
		);
	}
}
