using _305.Application.Base.Query;
using _305.Domain.Entity;

namespace _305.Application.Features.AdminUserFeatures.Query;

public class GetPaginatedAdminUserQuery : GetPaginatedQuery<User>
{
    public bool is_active { get; set; }
}
