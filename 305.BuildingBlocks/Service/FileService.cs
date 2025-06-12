using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Service;

/// <summary>
/// سرویس فایل برای بارگذاری و حذف تصاویر.
/// وابسته به HttpContext برای دسترسی به اطلاعات درخواست (مانند آدرس Base).
/// </summary>
public class FileService(IHttpContextAccessor contextAccessor) : IFileService
{
    /// <summary>
    /// حذف فایل تصویر از مسیر مشخص‌شده در URL
    /// </summary>
    /// <param name="imageUrl">آدرس نسبی فایل تصویر که باید حذف شود</param>
    public void DeleteImage(string imageUrl)
    {
        // استفاده از کلاس FileManager برای حذف امن فایل
        FileManager.DeleteImageFile(imageUrl);
    }

    /// <summary>
    /// آپلود فایل تصویر به سرور و بازگرداندن مسیر URL آن
    /// </summary>
    /// <param name="file">فایل تصویر دریافت‌شده از فرم</param>
    /// <returns>مسیر URL تصویر آپلودشده</returns>
    public async Task<string> UploadImage(IFormFile file)
    {
        // دریافت شیء HttpRequest برای استخراج مسیر دامنه جهت تولید URL کامل
        var request = contextAccessor.HttpContext?.Request
                      ?? throw new InvalidOperationException("HttpContext در دسترس نیست.");

        // استفاده از FileManager برای ذخیره تصویر و تولید URL نهایی
        return await FileManager.UploadImageAsync(file, request);
    }
}