using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.AdminAuthFeatures.Command;

public class AdminLoginCommand : IRequest<ResponseDto<LoginResponse>>
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفا مقدار {0}را وارد کنید.")]
    public required string email { get; set; }
    [Display(Name = "پسورد")]
    [Required(ErrorMessage = "لطفا مقدار {0}را وارد کنید.")]
    public required string password { get; set; }
}
