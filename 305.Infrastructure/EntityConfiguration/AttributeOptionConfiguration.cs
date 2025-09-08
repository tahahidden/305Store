using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class AttributeOptionConfiguration : IEntityTypeConfiguration<Domain.Entity.AttributeOption>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.AttributeOption> builder)
        {
            builder.HasOne(o => o.attribute)
                .WithMany(o => o.attributeOptions)
                .HasForeignKey(o => o.attributeId);
        }
    }
}