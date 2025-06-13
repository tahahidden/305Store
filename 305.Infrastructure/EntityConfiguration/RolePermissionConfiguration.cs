using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.slug).IsRequired().HasMaxLength(1000);;
        builder.HasIndex(x => x.slug).IsUnique();
        builder
            .HasOne(x => x.role)
            .WithMany(x => x.role_permissions)
            .HasForeignKey(x => x.role_id);

        builder
            .HasOne(x => x.permission)
            .WithMany(x => x.role_permissions)
            .HasForeignKey(x => x.permission_id);
    }
}