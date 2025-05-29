using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Domain.EntityConfiguration;
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{

		builder.HasKey(x => new { x.user_id, x.role_id, x.id });
		builder.Property(x => x.slug).IsRequired();
		builder.HasIndex(x => x.slug).IsUnique();
		builder
			.HasOne(x => x.user)
			.WithMany(x => x.user_roles)
			.HasForeignKey(x => x.user_id)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasOne(x => x.role)
			.WithMany(x => x.user_roles)
			.HasForeignKey(x => x.role_id)
			.OnDelete(DeleteBehavior.Cascade);

	}
}
