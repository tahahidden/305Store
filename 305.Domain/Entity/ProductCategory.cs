using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class ProductCategory : BaseEntity
    {
        public string? description { get; set; }
        public long? parentId { get; set; }

        public ProductCategory? productCategory { get; set; }
        public ICollection<ProductCategory>? productCategories { get; set; }
        public ICollection<Product>? products { get; set; }




        /// <summary>
        /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
        /// </summary>
        public ProductCategory(string name, string slug, string? description = null, long? parentId = null) : base(name, slug)
        {
            this.description = description;
            this.parentId = parentId;

        }

        public ProductCategory() { }
    }
}