namespace _305.BuildingBlocks.Configurations;

/// <summary>
/// پیکربندی موردنیاز برای ارتباط با سرویس پیامکی.
/// مقادیر این کلاس از فایل تنظیمات بارگذاری می‌شود.
/// </summary>
public class SmsConfig
{
    public const string SectionName = "Sms";

    /// <summary>
    /// کلید api کاوه نگار
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// شماره مورد استفاده در کاوه نگار
    /// </summary>
    public string SenderNumber { get; set; } = string.Empty;
}
