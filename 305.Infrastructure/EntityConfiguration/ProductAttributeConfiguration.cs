using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<Domain.Entity.ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.ProductAttribute> builder)
        {
            builder.HasOne(o => o.product)
                .WithMany(o => o.productAttributes)
                .HasForeignKey(o => o.productId);


            builder.HasOne(o => o.attribute)
           .WithMany(o => o.productAttributes)
           .HasForeignKey(o => o.attributeId);
        }
    }
}