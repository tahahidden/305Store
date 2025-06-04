using Microsoft.AspNetCore.Http;
using System.Text;

namespace _305.Tests.Unit.Assistant;

/// <summary>
/// ابزار ساخت فایل جعلی برای تست.
/// با استفاده از این کلاس می‌توانید فایل‌های تستی از نوع <see cref="IFormFile"/> بسازید.
/// </summary>
public static class Files
{
	/// <summary>
	/// ساخت فایل تستی <see cref="IFormFile"/> از محتوای متنی.
	/// </summary>
	/// <param name="fileName">نام فایل (پیش‌فرض: "test.jpg")</param>
	/// <param name="contentType">نوع فایل (پیش‌فرض: "image/jpeg")</param>
	/// <param name="content">محتوای رشته‌ای فایل (پیش‌فرض: "fake image content")</param>
	/// <returns>فایل جعلی قابل استفاده در تست‌ها</returns>
	public static IFormFile CreateFakeFormFile(string fileName = "test.jpg", string contentType = "image/jpeg", string content = "fake image content")
	{
		var bytes = Encoding.UTF8.GetBytes(content);
		return CreateFakeFormFile(bytes, fileName, contentType);
	}

	/// <summary>
	/// ساخت فایل تستی <see cref="IFormFile"/> از آرایه بایت.
	/// </summary>
	/// <param name="bytes">محتوای فایل به صورت آرایه بایت</param>
	/// <param name="fileName">نام فایل</param>
	/// <param name="contentType">نوع فایل</param>
	/// <returns>فایل جعلی قابل استفاده در تست‌ها</returns>
	public static IFormFile CreateFakeFormFile(byte[] bytes, string fileName, string contentType)
	{
		var stream = new MemoryStream(bytes);
		return new FormFile(stream, 0, stream.Length, name: "file", fileName: fileName)
		{
			Headers = new HeaderDictionary(),
			ContentType = contentType
		};
	}
}
