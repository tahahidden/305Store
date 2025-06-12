using _305.Application.Base.Command;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.UserRoleFeatures.Command;
public class CreateUserRoleCommand : CreateCommand<string>
{
    [Display(Name = "آیدی نقش")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public long roleid { get; set; }
    [Display(Name = "آیدی کاربر")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public long userid { get; set; }
}
