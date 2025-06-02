using _305.Application.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Base.Command;
/// <summary>
/// فرمان حذف (DeleteCommand) برای حذف یک موجودیت بر اساس آیدی.
/// این فرمان توسط MediatR هندل می‌شود و عملیات حذف را در لایه Application/Handler انجام می‌دهد.
/// </summary>
/// <remarks>
/// از این فرمان می‌توان به عنوان پایه برای حذف هر نوع موجودیتی استفاده کرد که با شناسه یکتا (id) مشخص می‌شود.
/// </remarks>
public class DeleteCommand : IRequest<ResponseDto<string>>
{
    /// <summary>
    /// شناسه یکتای موجودیتی که باید حذف شود.
    /// این فیلد الزامی است و در صورت عدم مقداردهی، خطای اعتبارسنجی برگردانده می‌شود.
    /// </summary>
    [Display(Name = "آیدی")]
    [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
    public long id { get; set; }
}