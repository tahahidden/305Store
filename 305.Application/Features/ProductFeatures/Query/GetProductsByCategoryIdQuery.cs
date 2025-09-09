using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;
using _305.Application.Features.ProductFeatures.Response;
using MediatR;

namespace _305.Application.Features.ProductFeatures.Query
{
    public class GetProductsByCategoryIdQuery : IRequest<ResponseDto<List<ProductResponse>>>
    {
        public long ProductCategoryId { get; set; }

        public GetProductsByCategoryIdQuery(long productCategoryId)
        {
            ProductCategoryId = productCategoryId;
        }
    }
}