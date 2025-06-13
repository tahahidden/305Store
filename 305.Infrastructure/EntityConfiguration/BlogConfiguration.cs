using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.description).IsRequired();
        builder.Property(x => x.name).IsRequired();
        builder.Property(x => x.slug).IsRequired().HasMaxLength(1000);;
        builder.Property(x => x.image).IsRequired();
        builder.Property(x => x.blog_text).IsRequired();
        builder.Property(x => x.keywords).IsRequired();
        builder.HasIndex(x => x.slug).IsUnique();

        builder.HasOne(x => x.blog_category)
            .WithMany(x => x.blogs)
            .HasForeignKey(x => x.blog_category_id)
            .OnDelete(DeleteBehavior.Cascade);

    }
}