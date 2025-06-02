using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.Text;

namespace _305.BuildingBlocks.Security;

/// <summary>
/// کلاس ایستا برای تولید SecurityStamp (مهر امنیتی) به‌صورت تصادفی و امن
/// </summary>
public static class StampGenerator
{
    /// <summary>
    /// ایجاد مهر امنیتی تصادفی با طول مشخص و حروف و اعداد مجاز
    /// </summary>
    /// <param name="length">تعداد کاراکترهای مهر امنیتی</param>
    /// <returns>رشته‌ای متشکل از حروف و اعداد به صورت حروف بزرگ</returns>
    public static string CreateSecurityStamp(int length)
    {
        // بررسی اینکه طول مهر معتبر باشد (بزرگتر از صفر)
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "طول باید بیشتر از صفر باشد.");

        return RandomGenerator
            .GenerateString(length, AllowedCharacters.AlphanumericCase) // استفاده از کاراکترهای الفبا و عددی
            .ToUpperInvariant(); // تبدیل به حروف بزرگ برای یکنواختی
    }
}
