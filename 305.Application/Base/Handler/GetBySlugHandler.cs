using _305.Application.Base.Response;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای واکشی یک موجودیت بر اساس مقدار Slug و تبدیل آن به DTO.
/// از الگوهای Generic، UnitOfWork و AutoMapper پشتیبانی می‌کند.
/// </summary>
public class GetBySlugHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    /// <summary>
    /// سازنده کلاس که وابستگی به <see cref="IUnitOfWork"/> را تزریق می‌کند.
    /// </summary>
    /// <param name="unitOfWork">واحد کاری برای دسترسی به منابع داده</param>
    public GetBySlugHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        // ستفاده از Log.ForContext<T>() برای مشخص شدن منبع لاگ در Serilog
        _logger = logger ?? Log.ForContext<CreateHandler>(); // Contextual logging
    }

    /// <summary>
    /// اجرای عملیات واکشی موجودیت بر اساس Slug و تبدیل آن به DTO.
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت (Entity) که از دیتابیس واکشی می‌شود</typeparam>
    /// <typeparam name="TDto">نوع DTO که داده نهایی به آن نگاشت می‌شود</typeparam>
    /// <param name="fetchFunc">تابع برای واکشی موجودیت از دیتابیس با استفاده از UnitOfWork</param>
    /// <param name="name">نام نمایشی برای موجودیت جهت نمایش پیام‌ها</param>
    /// <param name="notFoundMessage">پیام سفارشی در صورت عدم یافتن</param>
    /// <returns>
    /// <see cref="ResponseDto{TDto}"/> شامل داده نگاشته‌شده یا پیام خطا
    /// </returns>
    public async Task<ResponseDto<TDto>> Handle<TEntity, TDto>(
        Func<IUnitOfWork, Task<TEntity?>> fetchFunc,
        string? name,
        string? notFoundMessage)
        where TEntity : class
        where TDto : class, new()
    {
        try
        {
            // تلاش برای یافتن موجودیت از طریق تابع fetchFunc
            var entity = await fetchFunc(_unitOfWork);
            if (entity == null)
                return Responses.NotFound<TDto>(default, name, notFoundMessage);

            // نگاشت Entity به DTO
            var dto = Mapper.Mapper.Map<TEntity, TDto>(entity);
            return Responses.Data(dto);
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("عملیات ایجاد لغو شد توسط CancellationToken");
            return Responses.Fail<TDto>(default, "عملیات لغو شد", 499);
        }
        catch (Exception ex)
        {
            // ثبت خطا با استفاده از Serilog
            _logger.Error(ex, "خطا در زمان دریافت موجودیت بر اساس Slug: {Message}", ex.Message);
            return Responses.ExceptionFail<TDto>(default, null);
        }
    }
}

