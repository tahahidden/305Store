using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {

        builder.HasKey(x => new { x.userid, x.roleid, x.id });
        builder.Property(x => x.slug).IsRequired();
        builder.HasIndex(x => x.slug).IsUnique();
        builder
            .HasOne(x => x.user)
            .WithMany(x => x.user_roles)
            .HasForeignKey(x => x.userid)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.role)
            .WithMany(x => x.user_roles)
            .HasForeignKey(x => x.roleid)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
