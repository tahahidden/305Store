using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.AttributeOptionFeature.Command;
using _305.Application.Features.AttributeOptionFeature.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Product
{
    [ApiController]
    [Route("api/product/attribute")]
    public class AttributeOptionController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("create")]
        public Task<IActionResult> Create([FromForm] CreateAttributeOptionCommand command, CancellationToken cancellationToken) =>
       ExecuteCommand<CreateAttributeOptionCommand, ResponseDto<string>>(command, cancellationToken);

        [HttpGet("all")]
        public Task<IActionResult> GetAll([FromQuery] GetAllAttributeOptionQuery query, CancellationToken cancellationToken) =>
       ExecuteQuery(query, cancellationToken);
    }
}