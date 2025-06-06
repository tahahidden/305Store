namespace _305.Application.Base.Response;
/// <summary>
/// کلاس عمومی برای پاسخ‌دهی به درخواست‌ها که شامل پیام، داده، وضعیت موفقیت و کد پاسخ است.
/// </summary>
/// <typeparam name="T">نوع داده‌ای که در پاسخ بازگردانده می‌شود</typeparam>
public class ResponseDto<T>
{
    /// <summary>
    /// پیام پاسخ (موفقیت، خطا و غیره)
    /// </summary>
    public string? message { get; set; }

    /// <summary>
    /// داده اصلی پاسخ که ممکن است مقدار داشته یا null باشد
    /// </summary>
    public T? data { get; set; }

    /// <summary>
    /// نشان‌دهنده موفقیت یا عدم موفقیت عملیات
    /// </summary>
    public bool is_success { get; set; }

    /// <summary>
    /// کد وضعیت یا کد پاسخ (مانند HTTP status code یا کدهای داخلی)
    /// </summary>
    public long response_code { get; set; }
}
