using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Command;

namespace _305.Application.Features.ProductCategoryFeatures.Command
{
    public class CreateProductCategoryCommand : CreateCommand<string>
    {
        public string? description { get; set; }
        public long? parentId { get; set; }
    }
}