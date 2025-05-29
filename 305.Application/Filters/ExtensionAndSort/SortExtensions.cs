using _305.BuildingBlocks.Enums;
using _305.Domain.Common;

namespace _305.Application.Filters.ExtensionAndSort;
/// <summary>
/// کلاس اکستنشن برای اعمال مرتب‌سازی روی IQueryable
/// </summary>
public static class SortExtensions
{
	/// <summary>
	/// اعمال مرتب‌سازی بر اساس مقدار SortByEnum روی موجودیت‌هایی که IBaseEntity را پیاده‌سازی کرده‌اند
	/// </summary>
	/// <typeparam name="T">نوع موجودیت که باید کلاس و IBaseEntity باشد</typeparam>
	/// <param name="query">کوئری اولیه</param>
	/// <param name="sortBy">نوع مرتب‌سازی که می‌تواند مقدار null هم باشد</param>
	/// <returns>کوئری مرتب‌شده</returns>
	public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, SortByEnum? sortBy)
		where T : class, IBaseEntity
	{
		// بر اساس مقدار sortBy عملیات OrderBy یا OrderByDescending انجام می‌شود
		// اگر sortBy مقدار نداشته باشد یا نامشخص باشد، به صورت پیش‌فرض بر اساس شناسه (id) به صورت نزولی مرتب می‌کند
		return sortBy switch
		{
			SortByEnum.created_at => query.OrderBy(x => x.created_at),
			SortByEnum.created_at_descending => query.OrderByDescending(x => x.created_at),
			SortByEnum.updated_at => query.OrderBy(x => x.updated_at),
			SortByEnum.updated_at_descending => query.OrderByDescending(x => x.updated_at),
			SortByEnum.slug => query.OrderBy(x => x.slug),
			SortByEnum.slug_descending => query.OrderByDescending(x => x.slug),
			SortByEnum.name => query.OrderBy(x => x.name),
			SortByEnum.name_descending => query.OrderByDescending(x => x.name),
			_ => query.OrderByDescending(x => x.id)
		};
	}
}

