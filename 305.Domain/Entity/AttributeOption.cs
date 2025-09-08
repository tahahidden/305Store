using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class AttributeOption : BaseEntity
    {
        public long attributeId { get; set; }
        public required string value { get; set; }

        public Attribute? attribute { get; set; }



        /// <summary>
        /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
        /// </summary>
        public AttributeOption(string name, string slug, long attributeId, string value) : base(name, slug)
        {
            this.attributeId = attributeId;
            this.value = value;
        }

        public AttributeOption() { }
    }
}