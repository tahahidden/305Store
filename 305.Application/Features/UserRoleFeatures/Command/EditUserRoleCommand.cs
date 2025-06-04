using _305.Application.Base.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _305.Application.Features.UserRoleFeatures.Command;
public class EditUserRoleCommand : EditCommand<string>
{
	[Display(Name = "آیدی نقش")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long roleid { get; set; }
	[Display(Name = "آیدی کاربر")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long userid { get; set; }
}
