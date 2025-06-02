using _304.Net.Platform.Application.BlogFeatures.Command;
using _304.Net.Platform.Application.BlogFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers;
[Route("api/blog-Blog")]
[ApiController]
public class BlogBlogController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> Index(GetPaginatedBlogQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] CreateBlogCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return InvalidModelResponse();
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromForm] EditBlogCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return InvalidModelResponse();
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetBySlug([FromForm] GetBlogBySlugQuery query, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return InvalidModelResponse();
            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromForm] DeleteBlogCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return InvalidModelResponse();
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}