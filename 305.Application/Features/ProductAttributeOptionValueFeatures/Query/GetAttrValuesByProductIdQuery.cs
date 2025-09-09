using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.ProductAttributeOptionValueFeatures.Response;
using MediatR;

namespace _305.Application.Features.ProductAttributeOptionValueFeatures.Query
{
    public class GetAttrValuesByProductIdQuery
        : IRequest<ResponseDto<List<ProductAttributeOptionValueResponse>>>
    {
        public long productId { get; set; }

        public GetAttrValuesByProductIdQuery(long productId)
        {
            this.productId = productId;
        }
    }
}