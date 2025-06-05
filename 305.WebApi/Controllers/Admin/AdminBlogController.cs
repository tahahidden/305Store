using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Query;
using _305.Application.Features.BlogFeatures.Response;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/blog")]
[ApiController]
public class AdminBlogController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("list")]
    public Task<IActionResult> Index([FromQuery] GetPaginatedBlogQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("create")]
    public Task<IActionResult> Create([FromForm] CreateBlogCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<CreateBlogCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpPost("edit")]
    public Task<IActionResult> Edit([FromForm] EditBlogCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<EditBlogCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpGet("get")]
    public Task<IActionResult> GetBySlug([FromQuery] GetBlogBySlugQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("delete")]
    public Task<IActionResult> Delete([FromForm] DeleteBlogCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<DeleteBlogCommand, ResponseDto<string>>(command, cancellationToken);
}