using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Query;
using _305.Application.Features.AttributeFeatures.Response;

namespace _305.Application.Features.AttributeFeatures.Query
{
    public class GetAllAttributeQuery : GetAllQuery<AttributeResponse>
    {
        public string? dataTypeName { get; set; }

    }
}