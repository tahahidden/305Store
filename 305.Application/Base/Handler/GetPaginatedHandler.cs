using _305.Application.Base.Response;
using _305.Application.Filters.Pagination;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای دریافت داده‌های صفحه‌بندی‌شده (Paginated) از ریپازیتوری با استفاده از UnitOfWork.
/// </summary>
public class GetPaginatedHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    /// <summary>
    /// سازنده کلاس که وابستگی به <see cref="IUnitOfWork"/> را دریافت می‌کند.
    /// </summary>
    /// <param name="unitOfWork">واحد کاری برای دسترسی به داده‌ها</param>
    public GetPaginatedHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        // ستفاده از Log.ForContext<T>() برای مشخص شدن منبع لاگ در Serilog
        _logger = logger ?? Log.ForContext<CreateHandler>(); // Contextual logging
    }

    /// <summary>
    /// دریافت داده‌های صفحه‌بندی شده از ریپازیتوری.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity) داده‌ها</typeparam>
    /// <param name="getRepo">تابعی که عملیات واکشی داده‌های صفحه‌بندی شده را با UnitOfWork انجام می‌دهد</param>
    /// <returns>
    /// شیء <see cref="ResponseDto{PaginatedList}"/> شامل لیست صفحه‌بندی‌شده داده‌ها یا پیام خطا
    /// </returns>
    public async Task<ResponseDto<PaginatedList<TEntity>>> Handle<TEntity>(
        Func<IUnitOfWork, Task<PaginatedList<TEntity>>> getRepo)
        where TEntity : class
    {
        try
        {
            var result = await getRepo(_unitOfWork);
            return Responses.Data(result);
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("عملیات ایجاد لغو شد توسط CancellationToken");
            return Responses.Fail<PaginatedList<TEntity>>(default, "عملیات لغو شد", 499);
        }
        catch (Exception ex)
        {
            // ثبت خطا با استفاده از Serilog
            _logger.Error(ex, "خطا در زمان دریافت موجودیت بر اساس Slug: {Message}", ex.Message);
            return Responses.ExceptionFail<PaginatedList<TEntity>>(default, null);
        }
    }
}
