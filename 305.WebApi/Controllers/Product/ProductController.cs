using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.ProductFeatures.Command;
using _305.Application.Features.ProductFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _305.WebApi.Controllers.Product
{
    [ApiController]
    [Route("api/product/product")]

    public class ProductController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("create")]
        public Task<IActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken) =>
               ExecuteCommand<CreateProductCommand, ResponseDto<string>>(command, cancellationToken);

        [HttpGet("all")]
        public Task<IActionResult> GetAll([FromQuery] GetAllProductQuery query, CancellationToken cancellationToken) =>
       ExecuteQuery(query, cancellationToken);
    }
}