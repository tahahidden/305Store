using System.Security.Cryptography;
using System.Text;

namespace _305.BuildingBlocks.Helper;

// کلاس ایستا برای تولید رشته، عدد و کدهای تصادفی به صورت امن
public static class RandomGenerator
{
    // آرایه‌ای از ارقام ۰ تا ۹ که برای تولید کد عددی استفاده می‌شود
    private static readonly char[] DefaultDigits = "0123456789".ToCharArray();

    /// <summary>
    /// تولید رشته‌ای تصادفی با طول مشخص، از بین کاراکترهای مجاز و با پیشوند دلخواه
    /// </summary>
    /// <param name="length">تعداد کاراکترهای رشته</param>
    /// <param name="allowedCharacters">کاراکترهای مجاز برای استفاده در رشته</param>
    /// <param name="prefix">پیشوند اختیاری برای افزودن به ابتدای رشته</param>
    public static string GenerateString(int length, string allowedCharacters, string prefix = "")
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length)); // بررسی معتبر بودن طول

        if (string.IsNullOrEmpty(allowedCharacters))
            throw new ArgumentException("allowedCharacters باید شامل کاراکتر باشد.");

        var result = new StringBuilder(length);
        var buffer = new byte[sizeof(uint)]; // بافر برای ذخیره اعداد تصادفی

        using var rng = RandomNumberGenerator.Create(); // ایجاد تولیدکننده اعداد تصادفی امن
        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer); // دریافت بایت‌های تصادفی
            var num = BitConverter.ToUInt32(buffer, 0); // تبدیل بایت‌ها به عدد صحیح بدون علامت
            result.Append(allowedCharacters[(int)(num % allowedCharacters.Length)]); // انتخاب کاراکتر مجاز با استفاده از باقی‌مانده
        }

        return prefix + result.ToString(); // افزودن پیشوند و بازگرداندن رشته نهایی
    }

    /// <summary>
    /// تولید عددی تصادفی بین min و max
    /// </summary>
    /// <param name="max">بیشینه مقدار (خارج از بازه)</param>
    /// <param name="min">کمینه مقدار (شامل بازه)</param>
    public static int GenerateNumber(int max, int min = 0)
    {
        if (max <= min)
            throw new ArgumentOutOfRangeException(nameof(max), "max باید بزرگتر از min باشد.");

        return RandomNumberGenerator.GetInt32(min, max); // تولید عدد صحیح تصادفی بین min و max
    }

    /// <summary>
    /// تولید کد عددی تصادفی با طول مشخص (پیش‌فرض ۸ رقمی)
    /// </summary>
    public static string GenerateCode(int length = 8)
    {
        return GenerateString(length, new string(DefaultDigits)); // استفاده از متد GenerateString فقط با ارقام
    }
}
