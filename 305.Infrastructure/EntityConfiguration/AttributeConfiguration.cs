
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Domain.Entity.Attribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.Attribute> builder)
        {
           
        }
    }
}