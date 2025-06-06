using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IUOW;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای عملیات ایجاد (Create) موجودیت‌ها در لایه داده.
/// از الگوی Unit of Work و اعتبارسنجی سفارشی پشتیبانی می‌کند.
/// </summary>
/// <remarks>
/// این هندلر قابلیت استفاده مجدد در سناریوهای مختلف ایجاد (Create) را فراهم می‌کند.
/// با استفاده از لیستی از <see cref="ValidationItem"/> ابتدا بررسی می‌کند که آیا شرایطی مانع ایجاد موجودیت هست یا خیر.
/// در صورت اعتبارسنجی موفق، عملیات ایجاد از طریق delegate <paramref name="onCreate"/> انجام شده
/// و سپس Commit به پایگاه داده صورت می‌گیرد.
/// </remarks>
public class CreateHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    /// <summary>
    /// سازنده کلاس با تزریق UnitOfWork.
    /// </summary>
    public CreateHandler(IUnitOfWork unitOfWork, ILogger? logger = null)
    {
        _unitOfWork = unitOfWork;
        // ستفاده از Log.ForContext<T>() برای مشخص شدن منبع لاگ در Serilog
        _logger = logger ?? Log.ForContext<CreateHandler>(); // Contextual logging
    }

    /// <summary>
    /// اجرای عملیات ایجاد موجودیت با اعتبارسنجی و مدیریت تراکنش.
    /// </summary>
    /// <typeparam name="TResult">نوع نتیجه خروجی</typeparam>
    /// <param name="validations">لیست قوانین اعتبارسنجی برای بررسی تکراری بودن یا شرایط خاص</param>
    /// <param name="onCreate">تابع حاوی منطق ایجاد موجودیت</param>
    /// <param name="createMessage">پیام موفقیت در صورت موفق بودن عملیات</param>
    /// <param name="cancellationToken">توکن کنسل کردن عملیات</param>
    /// <returns>شیء <see cref="ResponseDto{TResult}"/> شامل نتیجه عملیات</returns>
    public async Task<ResponseDto<TResult>> HandleAsync<TResult>(
       IEnumerable<ValidationItem>? validations,
       Func<Task<TResult>> onCreate,
       string? successMessage = "عملیات با موفقیت انجام شد",
       CancellationToken cancellationToken = default)
    {
        try
        {
            // اعتبارسنجی اولیه
            if (validations != null)
            {
                foreach (var validation in validations)
                {
                    if (await validation.Rule())
                    {
                        return Responses.Exist<TResult>(default, null, validation.Value);
                    }
                }
            }

            // انجام عملیات ایجاد
            var result = await onCreate();
            // ذخیره تغییرات
            await _unitOfWork.CommitAsync(cancellationToken);

            return Responses.Success(result, successMessage, 201);
        }
        catch (OperationCanceledException)
        {
            _logger.Warning("عملیات ایجاد لغو شد توسط CancellationToken");
            return Responses.Fail<TResult>(default, "عملیات لغو شد", 499);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "خطا در ایجاد موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<TResult>(default, null);
        }
    }
}