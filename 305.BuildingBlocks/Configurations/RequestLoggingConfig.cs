namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// تنظیمات مربوط به لاگ‌برداری درخواست‌های ورودی.
/// </summary>
public class RequestLoggingConfig
{
    /// <summary>
    /// نام بخشی که در فایل تنظیمات برای این پیکربندی استفاده می‌شود.
    /// </summary>
    public const string SectionName = "RequestLogging";

    /// <summary>
    /// مسیر ذخیره فایل لاگ درخواست‌ها.
    /// </summary>
    public string FilePath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "logs", "requests.txt");
}

