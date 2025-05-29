using _305.Application.Base.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace _305.Application.Base.Query;

/// <summary>
/// کوئری عمومی برای دریافت یک رکورد از نوع <typeparamref name="T"/> بر اساس مقدار slug (نامک).
/// </summary>
/// <typeparam name="T">نوع داده‌ای که قرار است بازیابی شود.</typeparam>
public class GetBySlugQuery<T> : IRequest<ResponseDto<T>>
{
	/// <summary>
	/// نامک (Slug) مورد نظر برای جستجو.
	/// این فیلد اجباری است و باید مقداردهی شود.
	/// </summary>
	[Display(Name = "نامک")]
	[Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
	public string slug { get; set; }
}