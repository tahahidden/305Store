using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Command;

namespace _305.Application.Features.ProductFeatures.Command
{
    public class CreateProductCommand : CreateCommand<string>
    {
        public long productCategoryId { get; set; }
        public string? description { get; set; }
        public decimal? price { get; set; }
    }
}