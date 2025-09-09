using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _305.Domain.Common;

namespace _305.Domain.Entity
{
    public class Product : BaseEntity
    {
        public long productCategoryId { get; set; }
        public string? description { get; set; }
        public decimal? price { get; set; }


        public ProductCategory? productCategory { get; set; }
        public ICollection<ProductAttribute>? productAttributes { get; set; }
        public ICollection<CategoryProductRelation>? categoryProductRelations { get; set; }



        public Product(string name, string slug, long productCategoryId, string? description = null, decimal? price = null) : base(name, slug)
        {
            this.description = description;
            this.price = price;
            this.productCategoryId = productCategoryId;

        }

        public Product() { }
    }
}