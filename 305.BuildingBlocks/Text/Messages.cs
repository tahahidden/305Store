namespace _305.BuildingBlocks.Text;

/// <summary>
/// کلاس ثابت پیام‌ها برای استفاده در برنامه
/// شامل پیام‌های عمومی موفقیت، خطا، عدم وجود، وجود و ...
/// </summary>
public static class Messages
{
    /// <summary>
    /// پیام خطای غیرمنتظره
    /// </summary>
    public static string ExceptionFail() => "خطایی غیر منتظره رخ داد لطفا دوباره تلاش کنید";

    /// <summary>
    /// پیام عملیات ناموفق
    /// </summary>
    public static string Fail() => "عملیات ناموفق بود";

    /// <summary>
    /// پیام عملیات موفق
    /// </summary>
    public static string Success() => "عملیات موفق بود";

    /// <summary>
    /// پیام وقتی آیتم مورد نظر پیدا نشد
    /// اگر نام آیتم مشخص باشد، آن را نمایش می‌دهد، در غیر اینصورت پیام کلی می‌دهد
    /// </summary>
    public static string NotFound(string? name) =>
        !string.IsNullOrWhiteSpace(name) ? $"{name} پیدا نشد" : "آیتم پیدا نشد";

    /// <summary>
    /// پیام وقتی آیتم با مشخصه‌ای وجود دارد
    /// اگر نام آیتم مشخص باشد، آن را نمایش می‌دهد، در غیر اینصورت پیام کلی می‌دهد
    /// </summary>
    /// <param name="name">نام آیتم</param>
    /// <param name="property">نام ویژگی یا مشخصه‌ای که چک شده</param>
    public static string Exist(string? name, string property) =>
        !string.IsNullOrWhiteSpace(name) ? $"{name} با این {property} وجود دارد" : $"آیتم با این {property} وجود دارد";

    /// <summary>
    /// پیام تغییر وضعیت آیتم
    /// </summary>
    /// <param name="name">نام آیتم یا وضعیت</param>
    public static string ChangeStatus(string name) => $"وضعیت {name} تغییر کرد";

    /// <summary>
    /// پیام اعتبارسنجی برای مقدار وارد نشده
    /// </summary>
    /// <param name="name">نام فیلد یا مقدار مورد نظر</param>
    public static string Validate(string name) => $"مقدار {name} را وارد کنید";
}
