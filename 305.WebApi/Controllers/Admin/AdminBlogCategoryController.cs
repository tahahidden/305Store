using _305.Application.Base.Response;
using _305.Application.Features.BlogCategoryFeatures.Command;
using _305.Application.Features.BlogCategoryFeatures.Query;
using _305.Application.Features.BlogFeatures.Query;
using _305.WebApi.Assistants.Permission;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Admin;
[Route("api/admin/blog-category")]
[ApiController]
public class AdminBlogCategoryController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("list")]
    [Permission("لیست دسته بندی های وبلاگ")]
    public Task<IActionResult> Index([FromQuery] GetPaginatedBlogQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpGet("all")]
    public Task<IActionResult> GetAll([FromQuery] GetAllCategoryQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("create")]
    public Task<IActionResult> Create([FromForm] CreateCategoryCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<CreateCategoryCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpPost("edit")]
    public Task<IActionResult> Edit([FromForm] EditCategoryCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<EditCategoryCommand, ResponseDto<string>>(command, cancellationToken);

    [HttpGet("get")]
    public Task<IActionResult> GetBySlug([FromQuery] GetCategoryBySlugQuery query, CancellationToken cancellationToken) =>
        ExecuteQuery(query, cancellationToken);

    [HttpPost("delete")]
    public Task<IActionResult> Delete([FromForm] DeleteCategoryCommand command, CancellationToken cancellationToken) =>
        ExecuteCommand<DeleteCategoryCommand, ResponseDto<string>>(command, cancellationToken);
}