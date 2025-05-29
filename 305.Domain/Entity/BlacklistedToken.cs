using _305.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer;
public class BlacklistedToken : BaseEntity
{
	public string token { get; set; } // The JWT token string
	public DateTime expiry_date { get; set; } // The expiration date of the token
	public DateTime black_listed_on { get; set; } = DateTime.UtcNow; // When the token was blacklisted
}

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
