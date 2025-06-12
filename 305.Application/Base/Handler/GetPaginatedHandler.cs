using _305.Application.Base.Response;
using _305.Application.Base.Validator;
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
    /// <param name="logger">لاگر Serilog (اختیاری)</param>
    public GetPaginatedHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        _logger = logger ?? Log.ForContext<GetPaginatedHandler>(); // Contextual logging
    }

    /// <summary>
    /// دریافت داده‌های صفحه‌بندی شده از ریپازیتوری.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity) داده‌ها</typeparam>
    /// <param name="getRepo">تابعی که داده‌های صفحه‌بندی شده را با UnitOfWork واکشی می‌کند</param>
    /// <returns>
    /// شیء <see cref="ResponseDto{PaginatedList}"/> شامل داده‌های صفحه‌بندی شده یا پیام خطا
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
            return ExceptionHandlers.CancellationException<PaginatedList<TEntity>>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<PaginatedList<TEntity>>(ex, _logger);
        }
    }
}