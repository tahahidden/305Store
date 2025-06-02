using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Domain.EntityConfiguration;
public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
{
    public void Configure(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.name).IsRequired();
        builder.Property(x => x.slug).IsRequired();
        builder.HasIndex(x => x.slug).IsUnique();
    }
}