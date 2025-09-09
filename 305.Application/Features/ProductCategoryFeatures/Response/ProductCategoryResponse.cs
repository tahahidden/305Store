using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;

namespace _305.Application.Features.ProductCategoryFeatures.Response
{
    public class ProductCategoryResponse : BaseResponse
    {
        public string? description { get; set; }
        public long? parentId { get; set; }
    }
}