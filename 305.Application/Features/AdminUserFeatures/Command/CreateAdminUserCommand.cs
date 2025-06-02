using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.AdminUserFeatures.Command;

public class CreateAdminUserCommand : CreateCommand
{
	[Display(Name = "پسورد")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string password { get; set; }
	[Display(Name = "ایمیل")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string email { get; set; }
}