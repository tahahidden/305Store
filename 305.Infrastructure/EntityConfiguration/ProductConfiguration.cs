using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Domain.Entity.Product>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.Product> builder)
        {
            builder.HasOne(o => o.productCategory)
                .WithMany(o => o.products)
                .HasForeignKey(o => o.productCategoryId);
        }
    }
}