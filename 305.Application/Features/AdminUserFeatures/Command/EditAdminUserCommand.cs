using DataLayer.Base.Command;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.AdminUserFeatures.Command;

public class EditAdminUserCommand : EditCommand
{
	[Display(Name = "پسورد")]
	public string? password { get; set; }
	[Display(Name = "ایمیل")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string email { get; set; }
}
