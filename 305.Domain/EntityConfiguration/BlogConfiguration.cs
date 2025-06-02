using _305.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Domain.EntityConfiguration;
public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.description).IsRequired();
        builder.Property(x => x.name).IsRequired();
        builder.Property(x => x.slug).IsRequired();
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