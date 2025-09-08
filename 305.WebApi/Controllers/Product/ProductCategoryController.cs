using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.ProductCategoryFeatures.Command;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Product
{
    [ApiController]
    [Route("api/product/product-category")]
    public class ProductCategoryController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("create")]
        public Task<IActionResult> Create([FromForm] CreateProductCategoryCommand command, CancellationToken cancellationToken) =>
       ExecuteCommand<CreateProductCategoryCommand, ResponseDto<string>>(command, cancellationToken);

    }
}