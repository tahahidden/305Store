using System.ComponentModel.DataAnnotations;
using _305.Application.Base.Command;

namespace _305.Application.Features.AdminUserFeatures.Command;

public class CreateAdminUserCommand : CreateCommand<string>
{
	[Display(Name = "پسورد")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public required string password { get; set; }
	[Display(Name = "ایمیل")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public required string email { get; set; }
}