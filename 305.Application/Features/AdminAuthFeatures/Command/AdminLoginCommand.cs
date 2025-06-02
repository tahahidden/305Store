using _305.Application.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.AdminAuthFeatures.Command;

public class AdminLoginCommand : IRequest<ResponseDto<LoginResponse>>
{
	[Display(Name = "ایمیل")]
	[Required(ErrorMessage = "لطفا مقدار {0}را وارد کنید.")]
	public string email { get; set; }
	[Display(Name = "پسورد")]
	[Required(ErrorMessage = "لطفا مقدار {0}را وارد کنید.")]
	public string password { get; set; }
}
