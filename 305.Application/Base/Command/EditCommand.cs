using _305.Application.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Base.Command;
/// <summary>
/// فرمان ویرایش (EditCommand) برای به‌روزرسانی اطلاعات یک موجودیت.
/// این فرمان با استفاده از MediatR پردازش شده و نتیجه عملیات ویرایش را در قالب <see cref="ResponseDto{T}"/> بازمی‌گرداند.
/// </summary>
/// <remarks>
/// این کلاس برای سناریوهایی طراحی شده که نیاز به به‌روزرسانی یک رکورد بر اساس شناسه یکتا دارند.
/// اطلاعاتی نظیر نامک (slug)، نام و تاریخ به‌روزرسانی از ورودی دریافت می‌شوند.
/// </remarks>
public class EditCommand : IRequest<ResponseDto<string>>
{
    /// <summary>
    /// شناسه یکتای موجودیتی که باید ویرایش شود.
    /// </summary>
    [Display(Name = "آیدی")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public long id { get; set; }

    /// <summary>
    /// نامک (slug) یکتا جهت استفاده در URL یا SEO.
    /// مقداردهی اختیاری است.
    /// </summary>
    [Display(Name = "نامک")]
    public string? slug { get; set; }

    /// <summary>
    /// نام نمایشی یا عنوان موجودیت.
    /// مقدار الزامی است و نباید خالی باشد.
    /// </summary>
    [Display(Name = "نام")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public string name { get; set; }

    /// <summary>
    /// تاریخ و زمان آخرین ویرایش.
    /// به صورت پیش‌فرض برابر با زمان فعلی تنظیم می‌شود.
    /// </summary>
    [Display(Name = "زمان ویرایش")]
    public DateTime updated_at { get; set; } = DateTime.Now;
}

