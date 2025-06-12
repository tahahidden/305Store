using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;

/// <summary>
/// هندلر عمومی برای واکشی یک موجودیت بر اساس مقدار Slug و تبدیل آن به DTO.
/// این کلاس از الگوهای Generic، UnitOfWork و AutoMapper پشتیبانی می‌کند.
/// </summary>
public class GetBySlugHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    /// <summary>
    /// سازنده کلاس که وابستگی به <see cref="IUnitOfWork"/> را تزریق می‌کند.
    /// </summary>
    /// <param name="unitOfWork">واحد کاری برای دسترسی به منابع داده</param>
    /// <param name="logger">لاگر Serilog (اختیاری)</param>
    public GetBySlugHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        _logger = logger ?? Log.ForContext<GetBySlugHandler>(); // Contextual logging
    }

    /// <summary>
    /// واکشی موجودیت بر اساس Slug و تبدیل آن به DTO.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity) که از دیتابیس واکشی می‌شود</typeparam>
    /// <typeparam name="TDto">نوع DTO که داده نهایی به آن نگاشت می‌شود</typeparam>
    /// <param name="fetchFunc">تابعی که موجودیت را از دیتابیس با استفاده از UnitOfWork واکشی می‌کند</param>
    /// <param name="name">نام نمایشی موجودیت برای پیام‌های بازگشتی</param>
    /// <param name="notFoundMessage">پیام سفارشی در صورت عدم یافتن موجودیت</param>
    /// <returns>
    /// شیء <see cref="ResponseDto{TDto}"/> شامل داده نگاشته‌شده یا پیام خطا
    /// </returns>
    public async Task<ResponseDto<TDto>> Handle<TEntity, TDto>(
        Func<IUnitOfWork, Task<TEntity?>> fetchFunc,
        string? name = null,
        string? notFoundMessage = null)
        where TEntity : class
        where TDto : class, new()
    {
        try
        {
            var entity = await fetchFunc(_unitOfWork);
            if (entity == null)
            {
                var message = notFoundMessage ?? $"{name ?? "موجودیت"} یافت نشد.";
                return Responses.NotFound<TDto>(null, name, message);
            }

            var dto = Mapper.Mapper.Map<TEntity, TDto>(entity);
            return Responses.Data(dto);
        }
        catch (OperationCanceledException)
        {
            return ExceptionHandlers.CancellationException<TDto>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<TDto>(ex, _logger);
        }
    }
}
