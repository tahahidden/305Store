using Core.Base.Text;

namespace _305.Application.Base.Response;

/// <summary>
/// کلاس کمکی استاتیک برای ساخت پاسخ‌های استاندارد (ResponseDto) در عملیات‌های مختلف.
/// شامل پاسخ‌های موفقیت‌آمیز، خطا، عدم وجود داده، وجود تکراری و ...
/// </summary>
public static class Responses
{
    /// <summary>
    /// ایجاد پاسخ موفقیت‌آمیز با داده دلخواه، پیام و کد وضعیت (پیش‌فرض 200)
    /// </summary>
    public static ResponseDto<T> Success<T>(T? data = default, string? message = null, int code = 200)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? Messages.Success(),
            response_code = code
        };
    }

    /// <summary>
    /// ایجاد پاسخ موفقیت‌آمیز برای عملیات تغییر یا حذف با پیام و کد پیش‌فرض 204 (No Content)
    /// </summary>
    public static ResponseDto<T> ChangeOrDelete<T>(T? data = default, string? message = null, int code = 204)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? Messages.Success(),
            response_code = code
        };
    }

    /// <summary>
    /// ایجاد پاسخ شکست (Fail) با پیام و کد وضعیت پیش‌فرض 400 (Bad Request)
    /// </summary>
    public static ResponseDto<T> Fail<T>(T? data = default, string? message = null, int code = 400)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.Fail(),
            response_code = code
        };
    }

    /// <summary>
    /// ایجاد پاسخ شکست ناشی از Exception با پیام و کد پیش‌فرض 500 (Internal Server Error)
    /// </summary>
    public static ResponseDto<T> ExceptionFail<T>(T? data = default, string? message = null, int code = 500)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.ExceptionFail(),
            response_code = code
        };
    }

    /// <summary>
    /// ایجاد پاسخ شکست به دلیل وجود داده مشابه (تکراری) با پیام و کد 409 (Conflict)
    /// </summary>
    public static ResponseDto<T> Exist<T>(T? data, string? name, string property, string? message = null)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.Exist(name, property),
            response_code = 409 // Conflict
        };
    }

    /// <summary>
    /// ایجاد پاسخ شکست به دلیل پیدا نشدن داده با پیام و کد 404 (Not Found)
    /// </summary>
    public static ResponseDto<T> NotFound<T>(T? data, string? name = null, string? message = null)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.NotFound(name),
            response_code = 404
        };
    }

    /// <summary>
    /// ایجاد پاسخ موفقیت‌آمیز با داده و پیام دلخواه و کد وضعیت (پیش‌فرض 200)
    /// </summary>
    public static ResponseDto<T> Data<T>(T data, string? message = null, int code = 200)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = true,
            message = message ?? Messages.Success(),
            response_code = code
        };
    }

    /// <summary>
    /// ایجاد پاسخ شکست به دلیل عدم اعتبار داده (اعتبارسنجی ناموفق) با پیام و کد 400 (Bad Request)
    /// </summary>
    public static ResponseDto<T> NotValid<T>(T? data, string propName = "آیتم", string? message = null, int code = 400)
    {
        return new ResponseDto<T>
        {
            data = data,
            is_success = false,
            message = message ?? Messages.Validate(propName),
            response_code = code
        };
    }
}
