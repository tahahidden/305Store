namespace _305.Application.Features.AdminUserFeatures.Query;

public class GetPaginatedAdminUserQuery : GetPaginatedQuery<User>
{
	public bool is_active { get; set; }
}
