using _305.Application.Base.Command;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.BlogCategoryFeatures.Command;

public class EditCategoryCommand : EditCommand<string>
{
    [Display(Name = "توضیحات")]
    public string? description { get; set; }
}
