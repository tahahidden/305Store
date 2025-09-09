using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Command;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Handler;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Query;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Response;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Product
{
    [ApiController]
    [Route("api/product/product-attribute-option-value")]
    public class ProductAttributeOptionValueController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("create")]
        public Task<IActionResult> Create([FromForm] CreateProductAttributeOptionValueCommand command, CancellationToken cancellationToken) =>
     ExecuteCommand<CreateProductAttributeOptionValueCommand, ResponseDto<string>>(command, cancellationToken);


        [HttpPost("get-by-product")]
        public Task<IActionResult> GetByProduct([FromForm] GetAttrValuesByProductIdQuery query, CancellationToken cancellationToken) =>
        ExecuteCommand<GetAttrValuesByProductIdQuery, ResponseDto<List<ProductAttributeOptionValueResponse>>>(query, cancellationToken);


    }
}
