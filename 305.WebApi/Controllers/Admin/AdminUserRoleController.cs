using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/user-role")]
[ApiController]
public class AdminUserUserRoleController(IMediator mediator) : BaseController(mediator)
{
	[HttpGet("list")]
	public Task<IActionResult> Index([FromQuery] GetPaginatedUserRoleQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("create")]
	public Task<IActionResult> Create([FromForm] CreateUserRoleCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<CreateUserRoleCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpPost("edit")]
	public Task<IActionResult> Edit([FromForm] EditUserRoleCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<EditUserRoleCommand, ResponseDto<string>>(command, cancellationToken);

	[HttpGet("get")]
	public Task<IActionResult> GetBySlug([FromQuery] GetUserRoleBySlugQuery query, CancellationToken cancellationToken) =>
		ExecuteQuery(query, cancellationToken);

	[HttpPost("delete")]
	public Task<IActionResult> Delete([FromForm] DeleteUserRoleCommand command, CancellationToken cancellationToken) =>
		ExecuteCommand<DeleteUserRoleCommand, ResponseDto<string>>(command, cancellationToken);
}
