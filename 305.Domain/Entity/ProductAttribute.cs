using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class ProductAttribute : BaseEntity
    {
        public long productId { get; set; }
        public long attributeId { get; set; }
        public bool isRequired { get; set; }



        public Product? product { get; set; }
        public Attribute? attribute { get; set; }



        /// <summary>
        /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
        /// </summary>
        public ProductAttribute(string name, string slug, long productId, long attributeId, bool isRequired) : base(name, slug)
        {
            this.productId = productId;
            this.attributeId = attributeId;
            this.isRequired = isRequired;

        }

        public ProductAttribute() { }
    }
}
