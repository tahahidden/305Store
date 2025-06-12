using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _305.Tests.Unit.TestHandlers.UserRoleTests;
public class EditUserRoleCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldEditUserRole_WhenEntityExists()
	{
		var roleRepoMock = new Mock<IRepository<Role>>();
		roleRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Role, bool>>>()))
			.ReturnsAsync(true);
		var userRepoMock = new Mock<IRepository<User>>();
		userRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
			.ReturnsAsync(true);


		await EditHandlerTestHelper.TestEditSuccess(
			handlerFactory: (repo, uow) => new EditUserRoleCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: UserRoleDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,		
			existingEntity: UserRoleDataProvider.Row(name: "old", id: 1),
			assertUpdated: entity =>
			{
				Assert.Equal("Updated Name", entity.name);
			},
			setupUowMock: uow =>
			{
				uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
				uow.Setup(x => x.UserRepository).Returns(userRepoMock.Object);
			}
		);
	}


	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenEntityDoesNotExist()
	{
		await EditHandlerTestHelper.TestEditNotFound<EditUserRoleCommand, UserRole, EditUserRoleCommandHandler>(
			handlerFactory: (repo, uow) => new EditUserRoleCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: UserRoleDataProvider.Edit(id: 2),
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
		var userRepoMock = new Mock<IRepository<User>>();
		userRepoMock
			.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
			.ReturnsAsync(true);

		await EditHandlerTestHelper.TestEditCommitFail(
			handlerFactory: (repo, uow) => new EditUserRoleCommandHandler(uow, repo),
			execute: (handler, command, token) => handler.Handle(command, token),
			command: UserRoleDataProvider.Edit(name: "Updated Name", id: 1),
			entityId: 1,
			existingEntity: UserRoleDataProvider.Row(name: "old Name", id: 1),
			setupUowMock: uow =>
			{
				uow.Setup(x => x.RoleRepository).Returns(roleRepoMock.Object);
				uow.Setup(x => x.UserRepository).Returns(userRepoMock.Object);
			}
		);
	}
}
