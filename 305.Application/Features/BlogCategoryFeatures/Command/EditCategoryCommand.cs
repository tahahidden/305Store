using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.BlogCategoryFeatures.Command;

public class EditCategoryCommand : EditCommand
{
	[Display(Name = "توضیحات")]
	public string? description { get; set; }
}
