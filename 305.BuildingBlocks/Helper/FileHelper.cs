using _305.BuildingBlocks.Constants;
using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار کمکی برای مدیریت فایل‌های آپلود شده مانند تصویر.
/// </summary>
public static class FileManager
{
	/// <summary>
	/// آپلود تصویر به مسیر مشخص‌شده و تولید آدرس اینترنتی آن.
	/// </summary>
	/// <param name="file">فایل ارسالی از سمت کلاینت</param>
	/// <param name="request">آبجکت HTTP برای استخراج base URL</param>
	/// <param name="folderName">نام پوشه ذخیره‌سازی (پیش‌فرض: images)</param>
	/// <returns>URL نهایی تصویر بارگذاری‌شده</returns>
	public static async Task<string> UploadImageAsync(
		IFormFile file,
		HttpRequest request,
		string folderName = FileDefaults.DefaultFolderName)
	{
		if (file == null || file.Length == 0)
			throw new ArgumentException("فایل معتبر نیست.", nameof(file));

		// ساخت مسیر کامل پوشه ذخیره‌سازی (مثلاً /app/images)
		var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), folderName);

		if (!Directory.Exists(uploadDirectory))
			Directory.CreateDirectory(uploadDirectory);

		// تولید نام یکتا برای فایل
		var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
		var filePath = Path.Combine(uploadDirectory, uniqueFileName);

		// ذخیره فایل در مسیر مشخص‌شده
		await using var fileStream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(fileStream);

		// تولید آدرس اینترنتی برای فایل
		var baseUrl = $"{request.Scheme}://{request.Host}";
		var fileUrl = $"{baseUrl}/{folderName}/{uniqueFileName}";

		return fileUrl;
	}

	/// <summary>
	/// حذف تصویر از مسیر مشخص‌شده.
	/// </summary>
	/// <param name="imageUrl">آدرس کامل تصویر (مثلاً http://site.com/images/abc.jpg)</param>
	public static void DeleteImageFile(string imageUrl)
	{
		if (string.IsNullOrWhiteSpace(imageUrl))
			return;

		// استخراج مسیر نسبی از URL
		var relativePath = imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
			? new Uri(imageUrl).AbsolutePath.TrimStart('/')
			: imageUrl.TrimStart('/');

		// ساخت مسیر فیزیکی کامل روی سرور
		var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), FileDefaults.WwwRootFolder, relativePath);

		if (File.Exists(fullFilePath))
			File.Delete(fullFilePath);
	}
}