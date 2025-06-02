using _305.Application.Base.Pagination;
using _305.Domain.Common;
using Core.Base.EF;
using System.Linq;

namespace _305.Application.Base.ExtensionAndSort;

/// <summary>
/// کلاس اکستنشن برای اعمال فیلتر روی کوئری‌های IQueryable
/// </summary>
public static class FilterExtensions
{
    /// <summary>
    /// اعمال فیلتر جستجو بر اساس کلمه کلیدی روی فیلدهای name و slug
    /// فقط روی موجودیت‌هایی که اینترفیس IBaseEntity را پیاده‌سازی کرده‌اند قابل استفاده است
    /// </summary>
    /// <typeparam name="T">نوع موجودیت که باید کلاس و IBaseEntity باشد</typeparam>
    /// <param name="query">کوئری اولیه</param>
    /// <param name="filter">شی فیلتر شامل SearchTerm</param>
    /// <returns>کوئری فیلتر شده</returns>
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, DefaultPaginationFilter filter)
        where T : class, IBaseEntity
    {
        // اگر مقدار جستجو خالی یا فقط فاصله است، کوئری را دست نخورده برمی‌گرداند
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            // کلمه جستجو را به حروف کوچک و بدون فاصله اضافی تبدیل می‌کنیم
            var keyword = filter.SearchTerm.ToLower().Trim();

            // اعمال شرط جستجو روی فیلدهای name و slug
            query = query.Where(x =>
                x.name != null && x.name.ToLower().Contains(keyword) ||
                x.slug != null && x.slug.ToLower().Contains(keyword));
        }

        return query;
    }
}
