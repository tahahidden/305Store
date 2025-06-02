using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedToken>
{
	public void Configure(EntityTypeBuilder<BlacklistedToken> builder)
	{
		builder.Property(x => x.token).IsRequired();
		builder.Property(x => x.expiry_date).IsRequired();
		builder.Property(x => x.black_listed_on).IsRequired();

		builder.HasKey(x => x.id);
		builder.Property(x => x.slug).IsRequired();
		builder.HasIndex(x => x.slug).IsUnique();
	}
}
