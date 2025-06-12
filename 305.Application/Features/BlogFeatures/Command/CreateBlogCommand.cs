using _305.Application.Base.Command;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Features.BlogFeatures.Command;

public class CreateBlogCommand : CreateCommand<string>
{
    [Display(Name = "توضیحات")]
    public string description { get; set; }
    [Display(Name = "تصویر شاخص")]
    public IFormFile? image_file { get; set; }
    [Display(Name = "تصویر شاخص")]
    public string? image { get; set; }
    [Display(Name = "Image Alt")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string image_alt { get; set; }
    [Display(Name = "متن وبلاگ")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string blog_text { get; set; }
    [Display(Name = "آیا نمایش داده شود؟")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public bool show_blog { get; set; }
    [Display(Name = "کلمات کلیدی")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string keywords { get; set; }
    [Display(Name = "Meta Description")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string meta_description { get; set; }
    [Display(Name = "زمان تقریبی مطالعه")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public int estimated_read_time { get; set; }
    [Display(Name = "آیدی دسته بندی")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public long blog_category_id { get; set; }
}
