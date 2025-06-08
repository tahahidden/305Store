using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Command;
using _305.Application.Features.AdminAuthFeatures.Query;
using _305.Application.Features.AdminAuthFeatures.Response;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/auth")]
[ApiController]
public class AdminAuthController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("login")]
    public Task<IActionResult> Login([FromForm] AdminLoginCommand query, CancellationToken cancellationToken) =>
        ExecuteCommand<AdminLoginCommand, ResponseDto<LoginResponse>>(query, cancellationToken);

    [HttpPost("logout")]
    public Task<IActionResult> Logout([FromForm] AdminLogoutCommand query, CancellationToken cancellationToken) =>
        ExecuteCommand<AdminLogoutCommand, ResponseDto<string>>(query, cancellationToken);

    [HttpPost("create")]
    public Task<IActionResult> Refresh([FromForm] AdminRefreshCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<AdminRefreshCommand, ResponseDto<LoginResponse>>(command, cancellationToken);

    [HttpGet("profile")]
    public Task<IActionResult> Profile([FromQuery] GetUserProfileQuery query, CancellationToken cancellationToken) =>
        ExecuteCommand<GetUserProfileQuery, ResponseDto<UserResponse>>(query, cancellationToken);
}
