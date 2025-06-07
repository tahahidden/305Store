namespace _305.BuildingBlocks.Utils;

/// <summary>
/// کلاس پیام‌های عمومی ساختاریافته، برای استفاده در سراسر برنامه.
/// شامل پیام‌های مربوط به وضعیت عملیات، اعتبارسنجی، و موجودیت‌ها.
/// </summary>
public static class Messages
{
	#region Status

	/// <summary>
	/// عملیات با موفقیت انجام شد.
	/// </summary>
	/// <param name="message">پیام سفارشی (اختیاری)</param>
	public static string Success(string message = "عملیات موفق بود") => message;

	/// <summary>
	/// عملیات با شکست مواجه شد.
	/// </summary>
	/// <param name="message">پیام سفارشی (اختیاری)</param>
	public static string Fail(string message = "عملیات ناموفق بود") => message;

	/// <summary>
	/// خطایی غیرمنتظره رخ داد.
	/// </summary>
	/// <param name="message">پیام سفارشی (اختیاری)</param>
	public static string ExceptionFail(string message = "خطایی غیر منتظره رخ داد لطفا دوباره تلاش کنید") => message;

	/// <summary>
	/// وضعیت یک آیتم تغییر کرد.
	/// </summary>
	/// <param name="name">نام آیتم یا وضعیت</param>
	public static string Change(string name) => $"وضعیت {name} تغییر کرد";

	#endregion

	#region Validation

	/// <summary>
	/// مقدار فیلد الزامی وارد نشده است.
	/// </summary>
	/// <param name="name">نام فیلد</param>
	public static string Required(string name) => $"مقدار {name} را وارد کنید";

	#endregion

	#region Entity

	/// <summary>
	/// آیتم مورد نظر پیدا نشد.
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	public static string NotFound(string? name) =>
		string.IsNullOrWhiteSpace(name) ? "آیتم پیدا نشد" : $"{name} پیدا نشد";

	/// <summary>
	/// آیتمی با ویژگی مشخص شده از قبل وجود دارد.
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	/// <param name="property">ویژگی تکراری (مثل ایمیل، کد ملی)</param>
	public static string Exist(string? name, string property) =>
		string.IsNullOrWhiteSpace(name) ? $"آیتم با این {property} وجود دارد" : $"{name} با این {property} وجود دارد";

	/// <summary>
	/// آیتم با موفقیت ایجاد شد.
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	public static string Created(string? name = null) =>
		string.IsNullOrWhiteSpace(name) ? "آیتم با موفقیت ایجاد شد" : $"{name} با موفقیت ایجاد شد";

	/// <summary>
	/// آیتم با موفقیت حذف شد.
	/// </summary>
	/// <param name="name">نام آیتم (اختیاری)</param>
	public static string Deleted(string? name = null) =>
		string.IsNullOrWhiteSpace(name) ? "آیتم با موفقیت حذف شد" : $"{name} با موفقیت حذف شد";

	#endregion
}
