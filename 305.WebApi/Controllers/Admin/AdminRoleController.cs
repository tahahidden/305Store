using _305.Application.Base.Response;
using _305.Application.Features.RoleFeatures.Command;
using _305.Application.Features.RoleFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/role")]
[ApiController]
public class AdminRoleController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("list")]
    public Task<IActionResult> Index([FromQuery] GetPaginatedRoleQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpGet("all")]
    public Task<IActionResult> GetAll([FromQuery] GetAllRoleQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("create")]
    public Task<IActionResult> Create([FromForm] CreateRoleCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<CreateRoleCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpPost("edit")]
    public Task<IActionResult> Edit([FromForm] EditRoleCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<EditRoleCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpGet("get")]
    public Task<IActionResult> GetBySlug([FromQuery] GetRoleBySlugQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("delete")]
    public Task<IActionResult> Delete([FromForm] DeleteRoleCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<DeleteRoleCommand, ResponseDto<string>>(command, cancellationToken);
}

