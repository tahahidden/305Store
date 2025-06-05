using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.Features.PermissionFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/permission")]
[ApiController]
public class AdminPermissionController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet("list")]
	public Task<IActionResult> Index([FromQuery] GetPaginatedPermissionQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpGet("all")]
	public Task<IActionResult> GetAll([FromQuery] GetAllPermissionQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("create")]
	public Task<IActionResult> Create([FromForm] CreatePermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<CreatePermissionCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpPost("edit")]
	public Task<IActionResult> Edit([FromForm] EditPermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<EditPermissionCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpGet("get")]
	public Task<IActionResult> GetBySlug([FromQuery] GetPermissionBySlugQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("delete")]
	public Task<IActionResult> Delete([FromForm] DeletePermissionCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<DeletePermissionCommand, ResponseDto<string>>(command, cancellationToken);
}