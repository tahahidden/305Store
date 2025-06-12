namespace _305.BuildingBlocks.Utils;
/// <summary>
/// شامل کدهای ثابت مربوط به وضعیت‌های مختلف پاسخ HTTP است.
/// </summary>
public static class ResponseCode
{
	/// <summary>
	/// عملیات با موفقیت انجام شد (موفقیت‌آمیز).
	/// </summary>
	public const int Success = 200;

	/// <summary>
	/// عملیات موفق بود اما محتوایی برای بازگرداندن وجود ندارد.
	/// </summary>
	public const int NoContent = 204;

	/// <summary>
	/// درخواست نادرست است یا دارای خطای ساختاری می‌باشد.
	/// </summary>
	public const int BadRequest = 400;

	/// <summary>
	/// منبع مورد نظر پیدا نشد.
	/// </summary>
	public const int NotFound = 404;

	/// <summary>
	/// تعارضی در پردازش درخواست با وضعیت فعلی منبع وجود دارد.
	/// </summary>
	public const int Conflict = 409;

	/// <summary>
	/// خطای داخلی سرور در هنگام پردازش درخواست رخ داده است.
	/// </summary>
	public const int InternalServerError = 500;
}

