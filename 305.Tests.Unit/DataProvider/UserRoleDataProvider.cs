using _305.Application.Features.RoleFeatures.Command;

namespace _305.Tests.Unit.DataProvider;
public class UserRoleDataProvider
{
	public static CreateRoleCommand Create(string name = "role-name", string slug = "role-slug")
	=> new ()
	{
		name = name,
		slug = slug
	};
}
