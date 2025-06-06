using _305.Application.Filters.Pagination;
using _305.Domain.Common;
using System.Linq.Expressions;

namespace _305.Application.Filters.ExtensionAndSort;

/// <summary>
/// اکستنشن برای فیلتر کردن روی چندین فیلد متنی از نوع string با استفاده از عبارت جستجو (SearchTerm)
/// </summary>
public static class FilterExtensions
{
    /// <summary>
    /// فیلتر کردن یک کوئری بر اساس عبارت جستجو و چند فیلد متنی مشخص‌شده توسط توسعه‌دهنده.
    /// فیلدهایی که باید بررسی شوند، از طریق expression‌ها مشخص می‌شوند.
    /// </summary>
    /// <typeparam name="T">نوع موجودیت که باید کلاس و پیاده‌ساز IBaseEntity باشد</typeparam>
    /// <param name="query">کوئری اولیه که قرار است فیلتر روی آن اعمال شود</param>
    /// <param name="filter">شی فیلتر که شامل SearchTerm است</param>
    /// <param name="fields">لیستی از property selectorهایی که مشخص می‌کنند جستجو روی چه فیلدهایی انجام شود</param>
    /// <returns>کوئری فیلتر شده بر اساس SearchTerm روی فیلدهای مشخص‌شده</returns>
    public static IQueryable<T> ApplyFilter<T>(
        this IQueryable<T> query,
        DefaultPaginationFilter filter,
        params Expression<Func<T, string?>>[] fields)
        where T : class, IBaseEntity
    {
        // اگر SearchTerm خالی باشد یا هیچ فیلدی برای جستجو مشخص نشده باشد، فیلتر اعمال نمی‌شود
        if (string.IsNullOrWhiteSpace(filter.SearchTerm) || fields == null || fields.Length == 0)
            return query;

        // تبدیل عبارت جستجو به حروف کوچک و حذف فاصله‌های اضافی
        var keyword = filter.SearchTerm.ToLower().Trim();

        // ایجاد یک پارامتر برای موجودیت مورد نظر (مثل x در عبارت x => x.name)
        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? combinedExpression = null;

        // برای هر فیلدی که مشخص شده، یک شرط ساخته می‌شود
        foreach (var field in fields)
        {
            // اجرای expression برای دسترسی به property مورد نظر (مثلاً x => x.name)
            var member = Expression.Invoke(field, parameter);

            // صدا زدن متد ToLower روی مقدار فیلد
            var toLowerCall = Expression.Call(member, nameof(string.ToLower), Type.EmptyTypes);

            // صدا زدن متد Contains روی نتیجه‌ی ToLower
            var containsCall = Expression.Call(
                toLowerCall,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(keyword));

            // بررسی اینکه مقدار فیلد null نباشد (x.name != null)
            var nullCheck = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));

            // ترکیب شرط null نبودن و contains (x.name != null && x.name.ToLower().Contains(keyword))
            var fullCondition = Expression.AndAlso(nullCheck, containsCall);

            // ترکیب شرط‌ها با OR برای همه‌ی فیلدها
            combinedExpression = combinedExpression == null
                ? fullCondition
                : Expression.OrElse(combinedExpression, fullCondition);
        }

        // اگر شرط نهایی ساخته شده باشد، آن را به‌عنوان یک عبارت lambda به Where بده
        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        // کوئری فیلترشده را برمی‌گردانیم
        return query;
    }
}
