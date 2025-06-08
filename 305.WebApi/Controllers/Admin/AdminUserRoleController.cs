using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Command;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/user-role")]
[ApiController]
public class AdminUserRoleController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("create")]
    public Task<IActionResult> Create([FromForm] CreateUserRoleCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<CreateUserRoleCommand, ResponseDto<string>>(command, cancellationToken);

}
