using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.IService;

/// <summary>
/// اینترفیس سرویس مدیریت فایل‌ها (فعلاً محدود به تصاویر).
/// تعریف عملیات پایه برای آپلود و حذف فایل.
/// </summary>
public interface IFileService
{
	/// <summary>
	/// آپلود یک فایل تصویر به سرور و دریافت آدرس URL آن.
	/// </summary>
	/// <param name="file">فایل تصویر دریافتی از کلاینت</param>
	/// <returns>رشته‌ای حاوی مسیر یا URL فایل آپلود شده</returns>
	Task<string> UploadImage(IFormFile file);

	/// <summary>
	/// حذف تصویر با استفاده از آدرس آن در سیستم فایل یا URL.
	/// </summary>
	/// <param name="imageUrl">آدرس نسبی یا مطلق تصویر</param>
	void DeleteImage(string imageUrl);
}