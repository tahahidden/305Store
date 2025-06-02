using _305.Application.Features.UserRoleFeatures.Command;

namespace _305.Tests.Unit.DataProvider;
public class UserRoleDataProvider
{
	public static CreateUserRoleCommand Create()
	=> new CreateUserRoleCommand()
	{
		role_id = 1,
		user_id = 1,
	};
}
