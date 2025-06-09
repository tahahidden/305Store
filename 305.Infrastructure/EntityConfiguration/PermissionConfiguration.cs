using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.name).IsRequired();
        builder.Property(x => x.slug).IsRequired().HasMaxLength(1000);
        builder.HasIndex(x => x.slug).IsUnique();
        builder
            .HasMany(x => x.role_permissions)
            .WithOne(x => x.permission)
            .HasForeignKey(x => x.permission_id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}