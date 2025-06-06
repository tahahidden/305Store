using _305.Domain.Common;

namespace _305.Domain.Entity;
public class BlacklistedToken : BaseEntity
{
    public string token { get; set; } = default!; // The JWT token string
    public DateTime expiry_date { get; set; } // The expiration date of the token
    public DateTime black_listed_on { get; set; } = DateTime.Now; // When the token was blacklisted
}
