using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.Text;

namespace _305.BuildingBlocks.Security;

/// <summary>
/// کلاس ایستا برای تولید Security Stamp تصادفی و ایمن.
/// از کاراکترهای مشخص‌شده برای تولید رشته استفاده می‌کند.
/// خروجی به صورت حروف بزرگ انگلیسی است.
/// </summary>
public static class StampGenerator
{
    /// <summary>
    /// تولید مهر امنیتی (Security Stamp) تصادفی به طول دلخواه.
    /// برای تولید مقادیر امن مانند کلیدهای یکتا، توکن‌ها یا کد تأیید مناسب است.
    /// </summary>
    /// <param name="length">تعداد کاراکترهای رشته خروجی (باید بزرگ‌تر از صفر باشد)</param>
    /// <param name="allowedChars">
    /// رشته‌ای شامل کاراکترهای مجاز برای تولید (در صورت null بودن، مقدار پیش‌فرض AlphanumericCase استفاده می‌شود)
    /// </param>
    /// <returns>رشته‌ای تصادفی و امن، شامل فقط کاراکترهای مجاز و با حروف بزرگ</returns>
    public static string CreateSecurityStamp(int length, string? allowedChars = null)
    {
        // اعتبارسنجی ورودی: طول باید بیشتر از صفر باشد
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "طول باید بیشتر از صفر باشد.");

        // استفاده از کاراکترهای مجاز مشخص‌شده یا مقدار پیش‌فرض (اعداد و حروف کوچک و بزرگ)
        var chars = allowedChars ?? AllowedCharacters.AlphanumericCase;

        // تولید رشته‌ای تصادفی با استفاده از RandomGenerator
        var randomString = RandomGenerator.GenerateString(length, chars);

        // تبدیل نتیجه به حروف بزرگ برای استانداردسازی
        return randomString.ToUpperInvariant();
    }
}