using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<Domain.Entity.ProductCategory>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.ProductCategory> builder)
        {
            builder.HasOne(o => o.productCategory)
                .WithMany(o => o.productCategories)
                .HasForeignKey(o => o.parentId);
        }
    }
}