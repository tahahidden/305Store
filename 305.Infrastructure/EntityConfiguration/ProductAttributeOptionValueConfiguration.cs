using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class ProductAttributeOptionValueConfiguration : IEntityTypeConfiguration<Domain.Entity.ProductAttributeOptionValue>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.ProductAttributeOptionValue> builder)
        {
            builder.HasOne(o => o.productAttribute)
                .WithMany(o => o.productAttributeOptionValues)
                .HasForeignKey(o => o.productAttributeId);


            builder.HasOne(o => o.attributeOption)
           .WithMany(o => o.productAttributeOptionValues)
           .HasForeignKey(o => o.attributeOptionId);
        }
    }
}