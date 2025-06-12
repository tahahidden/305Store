using _305.BuildingBlocks.Constants;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار ساخت فایل جعلی <see cref="IFormFile"/> برای استفاده در تست‌ها.
/// </summary>
public static class FakeFileFactory
{
	/// <summary>
	/// ایجاد فایل تستی از محتوای متنی پیش‌فرض یا سفارشی.
	/// </summary>
	/// <param name="fileName">نام فایل. پیش‌فرض: "test.jpg"</param>
	/// <param name="contentType">نوع فایل. پیش‌فرض: "image/jpeg"</param>
	/// <param name="textContent">محتوای متنی داخل فایل. پیش‌فرض: "fake image content"</param>
	/// <returns>فایل جعلی از نوع <see cref="IFormFile"/></returns>
	public static IFormFile FromText(
		string fileName = FileDefaults.DefaultFileName,
		string contentType = FileDefaults.DefaultContentType,
		string textContent = FileDefaults.DefaultTextContent)
	{
		var bytes = Encoding.UTF8.GetBytes(textContent);
		return FromBytes(bytes, fileName, contentType);
	}

	/// <summary>
	/// ایجاد فایل تستی از محتوای باینری (byte array).
	/// </summary>
	/// <param name="fileBytes">محتوای فایل به صورت آرایه‌ای از بایت</param>
	/// <param name="fileName">نام فایل</param>
	/// <param name="contentType">نوع فایل (مثلاً "image/jpeg")</param>
	/// <returns>فایل جعلی از نوع <see cref="IFormFile"/></returns>
	public static IFormFile FromBytes(byte[] fileBytes, string fileName, string contentType)
	{
		var stream = new MemoryStream(fileBytes);

		var formFile = new FormFile(stream, 0, stream.Length, name: "file", fileName: fileName)
		{
			Headers = new HeaderDictionary(),
			ContentType = contentType
		};

		return formFile;
	}
}