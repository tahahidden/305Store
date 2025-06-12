using _305.Application.Base.Response;
using _305.Application.Features.AdminUserFeatures.Command;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/user")]
[ApiController]
public class AdminUserController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("list")]
    public Task<IActionResult> Index([FromQuery] GetPaginatedAdminUserQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("create")]
    public Task<IActionResult> Create([FromForm] CreateAdminUserCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<CreateAdminUserCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpPost("edit")]
    public Task<IActionResult> Edit([FromForm] EditAdminUserCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<EditAdminUserCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpGet("get")]
    public Task<IActionResult> GetBySlug([FromQuery] GetAdminUserBySlugQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("delete")]
    public Task<IActionResult> Delete([FromForm] DeleteAdminUserCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<DeleteAdminUserCommand, ResponseDto<string>>(command, cancellationToken);
}