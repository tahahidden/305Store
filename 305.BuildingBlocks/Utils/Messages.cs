namespace _305.BuildingBlocks.Utils;

/// <summary>
/// کلاس ثابت برای نگهداری پیام‌های عمومی برنامه.
/// این کلاس به صورت ساختاریافته (دسته‌بندی شده) پیام‌ها را در گروه‌های مختلف مانند وضعیت، اعتبارسنجی و موجودیت‌ها نگهداری می‌کند.
/// </summary>
public static class Messages
{

	// پیام‌های مربوط به وضعیت عملیات‌ها مانند موفقیت، شکست، خطا و ...
	#region status

	/// <summary>
	/// پیام موفقیت‌آمیز بودن عملیات
	/// </summary>
	public static string Success(string message = "عملیات موفق بود") => message;

	/// <summary>
	/// پیام شکست عملیات
	/// </summary>
	public static string Fail(string message = "عملیات ناموفق بود") => message;

	/// <summary>
	/// پیام خطای غیرمنتظره برای مواردی که استثنایی رخ داده است
	/// </summary>
	public static string ExceptionFail(string message = "خطایی غیر منتظره رخ داد لطفا دوباره تلاش کنید")
		=> message;

	/// <summary>
	/// پیام تغییر وضعیت یک آیتم (مانند کاربر، سفارش و ...)
	/// </summary>
	/// <param name="name">نام وضعیت یا آیتم مورد نظر</param>
	public static string Change(string name) => $"وضعیت {name} تغییر کرد";

	#endregion


	// پیام‌های مربوط به اعتبارسنجی (Validation)
	#region Validation

	/// <summary>
	/// پیام برای زمانی که مقدار فیلدی وارد نشده باشد
	/// </summary>
	/// <param name="name">نام فیلد یا ویژگی</param>
	public static string Required(string name) => $"مقدار {name} را وارد کنید";
	#endregion

	// پیام‌های مربوط به موجودیت‌ها (Entities)، مانند پیدا نشدن، وجود داشتن، ایجاد، حذف و ...
	#region Entity
	/// <summary>
	/// پیام وقتی آیتم مورد نظر پیدا نشود
	/// </summary>
	/// <param name="name">نام آیتم مورد نظر (اختیاری)</param>
	public static string NotFound(string? name) =>
		!string.IsNullOrWhiteSpace(name) ? $"{name} پیدا نشد" : "آیتم پیدا نشد";

	/// <summary>
	/// پیام وقتی آیتمی با یک ویژگی خاص قبلاً ثبت شده باشد (تکراری بودن)
	/// </summary>
	/// <param name="name">نام آیتم</param>
	/// <param name="property">نام ویژگی یا مشخصه تکراری (مثل ایمیل، کد ملی)</param>
	public static string Exist(string? name, string property) =>
		!string.IsNullOrWhiteSpace(name) ? $"{name} با این {property} وجود دارد" : $"آیتم با این {property} وجود دارد";

	/// <summary>
	/// پیام زمانی که آیتمی با موفقیت ایجاد می‌شود
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	/// TODO: هر چی فرانت خواست
	public static string Created(string? name = null) =>
		!string.IsNullOrWhiteSpace(name) ? $"{name} با موفقیت ایجاد شد" : "آیتم با موفقیت ایجاد شد";

	/// <summary>
	/// پیام زمانی که آیتمی با موفقیت حذف می‌شود
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	/// TODO: هر چی فرانت خواست
	public static string Deleted(string? name = null) =>
		!string.IsNullOrWhiteSpace(name) ? $"{name} با موفقیت حذف شد" : "آیتم با موفقیت حذف شد";
	#endregion
}
