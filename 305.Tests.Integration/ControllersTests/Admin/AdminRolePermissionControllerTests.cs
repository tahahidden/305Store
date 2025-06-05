using _305.Application.Base.Response;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Response;
using _305.Tests.Integration.Base;
using _305.Tests.Integration.Base.Helpers;
using _305.Tests.Integration.Base.TestController;
using _305.Tests.Unit.DataProvider;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace _305.Tests.Integration.ControllersTests.Admin;
[TestFixture]
public class AdminRolePermissionControllerTests : BaseControllerTests<CreateRolePermissionCommand, string, EditRolePermissionCommand, RolePermissionResponse>
{
	public AdminRolePermissionControllerTests()
	{
		BaseUrl = $"{BaseUrlProvider.AdminApi}role-permission";
	}

	protected override MultipartFormDataContent CreateCreateForm(CreateRolePermissionCommand dto)
	{
		return new MultipartFormDataContent
			{
				{ new StringContent(dto.name), "name" },
				{ new StringContent(dto.slug ?? "slug"), "slug" },
				{ new StringContent(dto.role_id.ToString()), "role_id" },
				{ new StringContent(dto.permission_id.ToString()), "permission_id" }
			};
	}

	protected override MultipartFormDataContent CreateEditForm(EditRolePermissionCommand dto)
	{
		return new MultipartFormDataContent
			{
				{ new StringContent(dto.id.ToString()), "id" },
				{ new StringContent(dto.name), "name" },
				{ new StringContent(dto.slug ?? "slug"), "slug" },
				{ new StringContent(dto.role_id.ToString()), "role_id" },
				{ new StringContent(dto.permission_id.ToString()), "permission_id" }
			};
	}

	[Test]
	public async Task Create_Should_Return_Success()
	{
		var helper = new TestDataHelper(Client);
		var permissionId = await helper.CreatePermissionAndReturnIdAsync();
		var roleId = await helper.CreateRoleAndReturnIdAsync();
		
		var createCommand = RolePermissionDataProvider.Create(name: "new-RolePermission", permissionId: permissionId, roleId: roleId);
		var slug = await CreateEntityAsync(createCommand);
		Assert.That(slug, Is.Not.Null);
	}

	[Test]
	public async Task Create_Should_Return_BadRequest_When_MissingName()
	{
		var form = new MultipartFormDataContent
			{
				{ new StringContent("slug-only"), "slug" }
			};

		var response = await Client.PostAsync($"{BaseUrl}/create", form);
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
	}

	[Test]
	public async Task Edit_Should_Return_Success()
	{
		var helper = new TestDataHelper(Client);
		var permissionId = await helper.CreatePermissionAndReturnIdAsync();
		var roleId = await helper.CreateRoleAndReturnIdAsync();
		var createCommand = RolePermissionDataProvider.Create(name: "edit-title", slug: "edit-slug", permissionId: permissionId, roleId: roleId);
		var slug = await CreateEntityAsync(createCommand);
		var RolePermission = await GetBySlugOrIdAsync(slug);

		var editCommand = RolePermissionDataProvider.Edit(name: "edited-title", id: RolePermission.id, slug: "edited-slug", permissionId: permissionId, roleId: roleId);
		var editForm = CreateEditForm(editCommand);
		var response = await Client.PostAsync($"{BaseUrl}/edit", editForm);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var editResult = JsonConvert.DeserializeObject<ResponseDto<string>>(json);
		Assert.That(editResult?.is_success, Is.True);
	}


	[Test]
	public async Task Index_Paginated_Should_Return_Success()
	{
		var response = await Client.GetAsync($"{BaseUrl}/list?page=1&pageSize=10");
		response.EnsureSuccessStatusCode();
	}

	[Test]
	public async Task List_Should_Return_Correct_PageSize()
	{
		var response = await Client.GetAsync($"{BaseUrl}/list?page=1&pageSize=5");
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = await DeserializePaginatedResponse(json);

		Assert.That(result?.is_success, Is.True);
		Assert.That(result!.data.Data.Count, Is.LessThanOrEqualTo(5));
	}

	[Test]
	public async Task GetBySlug_Should_Return_Success()
	{
		var helper = new TestDataHelper(Client);
		var permissionId = await helper.CreatePermissionAndReturnIdAsync();
		var roleId = await helper.CreateRoleAndReturnIdAsync();
		var createCommand = RolePermissionDataProvider.Create(name: "edit-title", slug: "new-slug", permissionId: permissionId, roleId: roleId);
		await CreateEntityAsync(createCommand);

		var response = await Client.GetAsync($"{BaseUrl}/get?slug=new-slug");
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<RolePermissionResponse>>(json);

		Assert.That(result?.is_success, Is.True);
		Assert.That(result?.data, Is.Not.Null);
	}

	[Test]
	public async Task GetBySlug_Should_Return_NotFound_When_Slug_NotExists()
	{
		var response = await Client.GetAsync($"{BaseUrl}/get?slug=not-exists-slug");
		var json = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<ResponseDto<RolePermissionResponse>>(json);

		Assert.That(result?.is_success, Is.False);
		Assert.That(result?.response_code, Is.EqualTo(404));
	}

	[Test]
	public async Task Delete_Should_Return_Success()
	{
		var helper = new TestDataHelper(Client);
		var permissionId = await helper.CreatePermissionAndReturnIdAsync();
		var roleId = await helper.CreateRoleAndReturnIdAsync();
		var createCommand = RolePermissionDataProvider.Create(name: "edit-title", slug: "edit-slug", permissionId: permissionId, roleId: roleId);
		var slug = await CreateEntityAsync(createCommand);
		var RolePermission = await GetBySlugOrIdAsync(slug);

		await DeleteEntityAsync(RolePermission.id);
	}
}

