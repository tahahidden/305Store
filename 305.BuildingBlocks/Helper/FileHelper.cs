using _305.BuildingBlocks.Constants;
using _305.BuildingBlocks.IService;
using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار کمکی برای مدیریت فایل‌های آپلود شده مانند فایل.
/// </summary>
public class FileManager : IFileManager
{
    /// <summary>
    /// آپلود فایل به مسیر مشخص‌شده و تولید آدرس اینترنتی آن.
    /// </summary>
    /// <param name="file">فایل ارسالی از سمت کلاینت</param>
    /// <param name="request">آبجکت HTTP برای استخراج base URL</param>
    /// <param name="folderName">نام پوشه ذخیره‌سازی (پیش‌فرض: FileDefaults.DefaultFolderName)</param>
    /// <returns>URL نهایی فایل بارگذاری‌شده</returns>
    public async Task<string> UploadFileAsync(
        IFormFile file,
        HttpRequest request,
        string folderName = FileDefaults.DefaultFolderName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("فایل معتبر نیست.", nameof(file));

        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var uploadDirectory = GetUploadDirectory(folderName);

        var uniqueFileName = GenerateUniqueFileName(file.FileName);
        var filePath = Path.Combine(uploadDirectory, uniqueFileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(fileStream);

        return BuildFileUrl(request, folderName, uniqueFileName);
    }

    private static string GetUploadDirectory(string folderName)
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        return directory;
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        return $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
    }

    private static string BuildFileUrl(HttpRequest request, string folderName, string fileName)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/{folderName}/{fileName}";
    }

    /// <summary>
    /// حذف فایل از مسیر مشخص‌شده.
    /// </summary>
    /// <param name="imageUrl">آدرس کامل فایل (مثلاً http://site.com/images/abc.jpg)</param>
    public void DeleteFile(string imageUrl)
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
