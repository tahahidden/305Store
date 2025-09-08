using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class ProductAttributeOptionValue : BaseEntity
    {
        public long productAttributeId { get; set; }
        public long? attributeOptionId { get; set; }


        public ProductAttribute? productAttribute { get; set; }
        public AttributeOption? attributeOption { get; set; }



        public ProductAttributeOptionValue(string name, string slug, long productAttributeId, long attributeOptionId) : base(name, slug)
        {
            this.productAttributeId = productAttributeId;
            this.attributeOptionId = attributeOptionId;

        }

        public ProductAttributeOptionValue() { }
    }
}