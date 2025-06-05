using _305.Application.Base.Command;
using _305.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _305.Application.Features.RolePermissionFeatures.Command;
public class CreateRolePermissionCommand : CreateCommand<string>
{
	[Display(Name = "آیدی نقش")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long role_id { get; set; }
	[Display(Name = "آیدی دسترسی")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public long permission_id { get; set; }
}
