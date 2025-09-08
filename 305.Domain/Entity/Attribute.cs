using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;
using _305.Domain.Enums;

namespace _305.Domain.Entity
{
    public class Attribute : BaseEntity
    {
        public DataTypeEnum dataType { get; set; }


        public Product? product { get; set; }
        public ICollection<AttributeOption>? attributeOptions { get; set; }
        public ICollection<ProductAttribute>? productAttributes { get; set; }



        /// <summary>
        /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
        /// </summary>
        public Attribute(string name, string slug, long productId, DataTypeEnum dataType, bool isRequired) : base(name, slug)
        {
            this.dataType = dataType;
        }

        public Attribute() { }
    }
}