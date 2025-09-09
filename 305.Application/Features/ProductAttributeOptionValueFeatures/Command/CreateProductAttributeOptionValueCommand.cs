using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Application.Base.Command;

namespace _305.Application.Features.ProductAttributeOptionValueFeatures.Command
{
    public class CreateProductAttributeOptionValueCommand : CreateCommand<string>
    {
        public long productAttributeId { get; set; }
        public long? attributeOptionId { get; set; }

    }
}