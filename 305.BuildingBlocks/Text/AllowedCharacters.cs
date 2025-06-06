namespace _305.BuildingBlocks.Text;

/// <summary>
/// مجموعه‌ای از کاراکترهای از پیش تعریف‌شده برای استفاده در تولید رشته‌های تصادفی
/// </summary>
public static class AllowedCharacters
{
    /// <summary>
    /// اعداد از 1 تا 9 (بدون صفر) — مناسب برای جلوگیری از شروع عدد با صفر
    /// </summary>
    public const string Numeric = "123456789";

    /// <summary>
    /// اعداد کامل از 0 تا 9
    /// </summary>
    public const string Numeric0 = "0123456789";

    /// <summary>
    /// ترکیب کامل اعداد و حروف انگلیسی (کوچک و بزرگ)
    /// </summary>
    public const string AlphanumericCase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// فقط حروف انگلیسی (کوچک و بزرگ) — بدون اعداد
    /// </summary>
    public const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// حروف بزرگ انگلیسی به همراه اعداد — مناسب برای کدهایی با خوانایی بهتر
    /// </summary>
    public const string AlphanumericUpper = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// کاراکترهای قابل خواندن (بدون i, l, o, q برای جلوگیری از اشتباه دیداری)
    /// </summary>
    public const string AlphanumericReadable = "0123456789abcdefghkmnprstuvwyz";
}
