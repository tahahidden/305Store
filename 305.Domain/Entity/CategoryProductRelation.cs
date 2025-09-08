using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class CategoryProductRelation : BaseEntity
    {
        public long productCategoryId { get; set; }
        public long productId { get; set; }



        public Product? product { get; set; }
        public ProductCategory? productCategory { get; set; }



        /// <summary>
        /// سازنده برای ایجاد دسته‌بندی وبلاگ با مقادیر اولیه
        /// </summary>
        public CategoryProductRelation(string name, string slug, long productId, long productCategoryId, bool isRequired) : base(name, slug)
        {
            this.productId = productId;
            this.productCategoryId = productCategoryId;
        }

        public CategoryProductRelation() { }
    }
}