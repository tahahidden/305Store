using _305.Application.Base.Response;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.Features.RolePermissionFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/role-permission")]
[ApiController]
public class AdminRolePermissionController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet("list")]
	public Task<IActionResult> Index([FromQuery] GetPaginatedRolePermissionQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("create")]
	public Task<IActionResult> Create([FromForm] CreateRolePermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<CreateRolePermissionCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpPost("edit")]
	public Task<IActionResult> Edit([FromForm] EditRolePermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<EditRolePermissionCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpGet("get")]
	public Task<IActionResult> GetBySlug([FromQuery] GetRolePermissionBySlugQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("delete")]
	public Task<IActionResult> Delete([FromForm] DeleteRolePermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<DeleteRolePermissionCommand, ResponseDto<string>>(command, cancellationToken);
}