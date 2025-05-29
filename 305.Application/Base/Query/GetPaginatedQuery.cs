using _305.Application.Base.Response;
using _305.Application.Filters.Pagination;
using _305.BuildingBlocks.Enums;
using MediatR;

namespace _305.Application.Base.Query;
/// <summary>
/// کوئری عمومی برای دریافت لیست صفحه‌بندی شده از نوع <typeparamref name="TResponse"/> با امکان جستجو و مرتب‌سازی.
/// </summary>
/// <typeparam name="TResponse">نوع داده‌ای که قرار است صفحه‌بندی شود.</typeparam>
public class GetPaginatedQuery<TResponse> : IRequest<ResponseDto<PaginatedList<TResponse>>>
{
	/// <summary>
	/// عبارت جستجو برای فیلتر کردن داده‌ها (اختیاری).
	/// </summary>
	public string? SearchTerm { get; set; }

	/// <summary>
	/// معیار مرتب‌سازی داده‌ها. مقدار پیش‌فرض مرتب‌سازی بر اساس تاریخ ایجاد است.
	/// </summary>
	public SortByEnum SortBy { get; set; } = SortByEnum.created_at;

	/// <summary>
	/// تعداد آیتم‌ها در هر صفحه. مقدار پیش‌فرض 10 است.
	/// </summary>
	public int PageSize { get; set; } = 10;

	/// <summary>
	/// شماره صفحه جاری. مقدار پیش‌فرض 1 است.
	/// </summary>
	public int Page { get; set; } = 1;
}