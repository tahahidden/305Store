using System.Security.Cryptography;

namespace _305.BuildingBlocks.Security;

// کلاس ایستا برای هش کردن رمز عبور و بررسی اعتبار آن
public static class PasswordHasher
{
    // اندازه نمک (Salt) به بایت - ۱۶ بایت معادل ۱۲۸ بیت
    private const int SaltSize = 16;

    // اندازه کلید مشتق‌شده (هش) به بایت - ۳۲ بایت معادل ۲۵۶ بیت
    private const int KeySize = 32;

    // تعداد تکرار الگوریتم مشتق‌سازی برای تقویت امنیت
    private const int Iterations = 10000;

    // الگوریتم هش مورد استفاده (SHA512)
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    // متد برای هش کردن رمز عبور
    public static string Hash(string password)
    {
        // ایجاد نمونه از Rfc2898DeriveBytes با رمز، اندازه نمک، تعداد تکرار و الگوریتم
        using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithm);

        // استخراج کلید مشتق‌شده (هش نهایی) و تبدیل به Base64
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));

        // استخراج نمک تولید شده و تبدیل به Base64
        var salt = Convert.ToBase64String(algorithm.Salt);

        // بازگرداندن کلید و نمک به صورت رشته‌ای با نقطه جدا شده
        return $"{key}.{salt}";
    }

    // متد بررسی رمز عبور با استفاده از هش ذخیره شده
    public static bool Check(string hash, string password)
    {
        // اگر هش خالی یا نال باشد، بازگرداندن false
        if (string.IsNullOrWhiteSpace(hash)) return false;

        // جدا کردن رشته هش به دو بخش: کلید و نمک
        var parts = hash.Split('.', 2);
        if (parts.Length != 2)
            throw new FormatException("فرمت هش نامعتبر است. باید به صورت 'key.salt' باشد.");

        // تبدیل کلید و نمک از Base64 به بایت آرایه
        var key = Convert.FromBase64String(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);

        // ایجاد نمونه جدید از Rfc2898DeriveBytes با رمز عبور و نمک بازیابی‌شده
        using var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);

        // استخراج کلید مشتق‌شده جدید برای مقایسه
        var keyToCheck = algorithm.GetBytes(KeySize);

        // مقایسه دو کلید به صورت زمان-ثابت برای جلوگیری از حمله زمان‌سنجی
        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}
