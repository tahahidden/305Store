using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.id);
        builder.Property(x => x.slug).IsRequired().HasMaxLength(1000);;
        builder.HasIndex(x => x.slug).IsUnique();
        builder.Property(b => b.name).IsRequired();

        #region Navigations

        builder
            .HasMany(x => x.user_roles)
            .WithOne(x => x.role)
            .HasForeignKey(x => x.roleid)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

    }
}

