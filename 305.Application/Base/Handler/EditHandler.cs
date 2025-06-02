using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.Domain.Common;
using Serilog;

namespace _305.Application.Base.Handler;
/// <summary>
/// هندلر عمومی برای ویرایش (Edit) موجودیت‌ها در لایه داده.
/// با استفاده از الگوی Generic، Unit of Work و Repository پیاده‌سازی شده است.
/// </summary>
/// <typeparam name="TCommand">نوع کامند (در صورت نیاز برای توسعه بیشتر)</typeparam>
/// <typeparam name="TEntity">نوع موجودیت که باید از <see cref="IBaseEntity"/> ارث‌بری کند و شامل شناسه باشد</typeparam>
public class EditHandler<TCommand, TEntity>
    where TEntity : class, IBaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<TEntity> _repository;

    /// <summary>
    /// سازنده کلاس با تزریق وابستگی‌ها
    /// </summary>
    /// <param name="unitOfWork">واحد کاری برای مدیریت تراکنش‌ها</param>
    /// <param name="repository">مخزن داده برای یافتن موجودیت</param>
    public EditHandler(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    /// <summary>
    /// اجرای عملیات ویرایش موجودیت با اعمال اعتبارسنجی، عملیات قبل و بعد از ویرایش، و مدیریت تراکنش.
    /// </summary>
    /// <param name="id">شناسه موجودیت برای ویرایش</param>
    /// <param name="validations">لیست اعتبارسنجی‌های لازم پیش از ویرایش (اختیاری)</param>
    /// <param name="updateEntity">تابعی که منطق به‌روزرسانی موجودیت را اعمال می‌کند</param>
    /// <param name="beforeUpdate">تابع اختیاری برای اجرای عملیات قبل از ویرایش (مثلاً آماده‌سازی)</param>
    /// <param name="afterUpdate">تابع اختیاری برای اجرای عملیات بعد از ویرایش (مثلاً ثبت لاگ)</param>
    /// <param name="propertyName">نام موجودیت جهت درج در پیام‌ها</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>
    /// شیء <see cref="ResponseDto{string}"/> شامل پیام موفقیت یا خطا و وضعیت عملیات.
    /// </returns>
    public async Task<ResponseDto<string>> HandleAsync(
        long id,
        List<ValidationItem>? validations,
        Func<TEntity, Task<string>> updateEntity,
        Func<TEntity, Task>? beforeUpdate = null,
        Func<TEntity, Task>? afterUpdate = null,
        string propertyName = "رکورد",
        CancellationToken cancellationToken = default)
    {
        try
        {
            // یافتن موجودیت بر اساس شناسه
            var entity = await _repository.FindSingle(x => x.id == id);
            if (entity == null)
                return Responses.NotFound<string>(default, propertyName);

            // بررسی اعتبارسنجی‌ها (در صورت وجود)
            if (validations != null)
            {
                foreach (var validation in validations)
                {
                    var isValid = await validation.Rule();
                    if (isValid)
                        return Responses.Exist<string>(default, null, validation.Value);
                }
            }

            // عملیات اختیاری قبل از ویرایش
            if (beforeUpdate != null)
                await beforeUpdate(entity);

            // اعمال منطق ویرایش روی موجودیت
            var result = await updateEntity(entity);

            // عملیات اختیاری بعد از ویرایش
            if (afterUpdate != null)
                await afterUpdate(entity);

            // ذخیره تغییرات در دیتابیس
            var committed = await _unitOfWork.CommitAsync(cancellationToken);
            if (!committed)
                return Responses.ExceptionFail(result, $"{propertyName} ویرایش نشد", 500);

            return Responses.Success<string>(result, "ویرایش با موفقیت انجام شد", 200);
        }
        catch (Exception ex)
        {
            // ثبت خطای احتمالی با Serilog
            Log.Error(ex, "خطا در زمان ویرایش موجودیت: {Message}", ex.Message);
            return Responses.ExceptionFail<string>(default, null);
        }
    }
}

