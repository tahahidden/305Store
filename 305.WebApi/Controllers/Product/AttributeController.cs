using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.AttributeFeatures.Command;
using _305.Application.Features.AttributeFeatures.Query;
using _305.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _305.WebApi.Controllers.Product
{
    [ApiController]
    [Route("api/product/attribute")]
    public class AttributeController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost("create")]
        public Task<IActionResult> Create([FromForm] CreateAttributeCommand command, CancellationToken cancellationToken) =>
       ExecuteCommand<CreateAttributeCommand, ResponseDto<string>>(command, cancellationToken);

        [HttpGet("all")]
        public Task<IActionResult> GetAll([FromQuery] GetAllAttributeQuery query, CancellationToken cancellationToken) =>
       ExecuteQuery(query, cancellationToken);
    }
}