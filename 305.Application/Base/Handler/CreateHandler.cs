using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;

/// <summary>
/// هندلر عمومی برای عملیات ایجاد (Create) موجودیت.
/// این کلاس با استفاده از UnitOfWork و لیستی از قوانین اعتبارسنجی، عملیات ایجاد را با مدیریت تراکنش و لاگ انجام می‌دهد.
/// </summary>
public class CreateHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    /// <summary>
    /// سازنده هندلر ایجاد.
    /// </summary>
    /// <param name="unitOfWork">واحد کاری جهت مدیریت تراکنش‌ها</param>
    /// <param name="logger">لاگر (اختیاری، در صورت عدم ارسال از Serilog ایجاد می‌شود)</param>
    public CreateHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        _logger = logger ?? Log.ForContext<CreateHandler>();
    }

    /// <summary>
    /// اجرای عملیات ایجاد موجودیت به‌همراه اعتبارسنجی و مدیریت تراکنش.
    /// </summary>
    /// <typeparam name="TResult">نوع نتیجه خروجی</typeparam>
    /// <param name="validations">لیستی از قوانین اعتبارسنجی (مانند بررسی تکراری بودن یا موجود بودن آیتم‌ها)</param>
    /// <param name="onCreate">تابع اصلی حاوی منطق ایجاد موجودیت</param>
    /// <param name="successMessage">پیام موفقیت در صورت موفق بودن عملیات</param>
    /// <param name="cancellationToken">توکن کنسل کردن عملیات</param>
    /// <returns>نتیجه عملیات به صورت <see cref="ResponseDto{TResult}"/></returns>
    public async Task<ResponseDto<TResult>> HandleAsync<TResult>(
        IEnumerable<ValidationItem>? validations,
        Func<Task<TResult>> onCreate,
        string? successMessage = "عملیات با موفقیت انجام شد",
        CancellationToken cancellationToken = default)
    {
        try
        {
            // اجرای اعتبارسنجی‌ها
            if (validations != null)
            {
                foreach (var validation in validations)
                {
                    if (!await validation.Rule()) continue;

                    // اگر شرط برقرار بود، خطا صادر شود (Exist یا NotFound)
                    return validation.IsExistRole
                        ? Responses.Exist<TResult>(default, null, validation.Value)
                        : Responses.NotFound<TResult>(default, validation.Value);
                }
            }

            // اجرای عملیات ایجاد
            var result = await onCreate();

            // ذخیره تغییرات در پایگاه‌داده
            await _unitOfWork.CommitAsync(cancellationToken);

            // بازگشت نتیجه موفق
            return Responses.Success(result, successMessage, 201);
        }
        catch (OperationCanceledException)
        {
            return ExceptionHandlers.CancellationException<TResult>(_logger);
        }
        catch (Exception ex)
        {
            return ExceptionHandlers.GeneralException<TResult>(ex, _logger);
        }
    }
}
