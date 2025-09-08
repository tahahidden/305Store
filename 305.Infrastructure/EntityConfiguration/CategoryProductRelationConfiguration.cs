using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class CategoryProductRelationConfiguration : IEntityTypeConfiguration<Domain.Entity.CategoryProductRelation>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.CategoryProductRelation> builder)
        {
            builder.HasOne(o => o.product)
                 .WithMany(o => o.categoryProductRelations)
                 .HasForeignKey(o => o.productId);


            builder.HasOne(o => o.productCategory)
           .WithMany(o => o.categoryProductRelations)
           .HasForeignKey(o => o.productCategoryId);
        }
    }
}