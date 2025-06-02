using GoldAPI.Application.OptionFeatures.Command;
using GoldAPI.Application.UserRoleFeatures.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldAPI.Test.DataProvider;
public class UserRoleDataProvider
{
	public static CreateUserRoleCommand Create()
	=> new CreateUserRoleCommand()
	{
		role_id = 1,
		user_id = 1,
	};
}
