using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Response;

namespace _305.Application.Features.ProductAttributeOptionValueFeatures.Response
{
    public class ProductAttributeOptionValueDto : BaseResponse
    {
        public string? AttributeName { get; set; }
        public string? OptionName { get; set; }
    }
}