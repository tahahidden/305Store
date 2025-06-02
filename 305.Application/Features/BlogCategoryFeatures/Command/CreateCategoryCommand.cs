using System.ComponentModel.DataAnnotations;
using _305.Application.Base.Command;

namespace _305.Application.Features.BlogCategoryFeatures.Command;

public class CreateCategoryCommand : CreateCommand<string>
{
	[Display(Name = "توضیحات")]
	public string? description { get; set; }
}
