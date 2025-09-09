using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;

namespace _305.Application.Features.ProductFeatures.Response
{
    public class ProductResponse : BaseResponse
    {
        public string? description { get; set; }
        public decimal? price { get; set; }
    }
}