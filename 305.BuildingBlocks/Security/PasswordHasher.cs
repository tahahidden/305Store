using _305.BuildingBlocks.Configurations;
using System.Security.Cryptography;

namespace _305.BuildingBlocks.Security;

/// <summary>
/// کلاس ایستا برای هش کردن رمز عبور و بررسی اعتبار آن با استفاده از الگوریتم PBKDF2
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// هش کردن رمز عبور به همراه ذخیره نمک و پارامترها در خروجی نهایی
    /// </summary>
    /// <param name="password">رمز عبور خام</param>
    /// <returns>رشته هش شامل پارامترها: iterations.algorithm.key.salt</returns>
    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(HashConfig.SaltSize); // تولید نمک تصادفی

        // اجرای الگوریتم مشتق‌سازی کلید
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            HashConfig.DefaultIterations,
            HashConfig.DefaultAlgorithm,
            HashConfig.KeySize);

        // تبدیل کلید و نمک به base64 و قالب‌بندی با پارامترها
        return $"{HashConfig.DefaultIterations}.{HashConfig.DefaultAlgorithm}.{Convert.ToBase64String(key)}.{Convert.ToBase64String(salt)}";
    }

    /// <summary>
    /// بررسی صحت رمز عبور با استفاده از هش ذخیره‌شده
    /// </summary>
    /// <param name="hash">هش ذخیره شده شامل iterations.algorithm.key.salt</param>
    /// <param name="password">رمز عبور خام ورودی</param>
    /// <returns>درستی یا نادرستی رمز عبور</returns>
    public static bool Check(string hash, string password)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return false;

        // جدا کردن قسمت‌های هش: تکرار، الگوریتم، کلید، نمک
        var parts = hash.Split('.', 4);
        if (parts.Length != 4)
            throw new FormatException("فرمت هش نامعتبر است. ساختار صحیح: iterations.algorithm.key.salt");

        // بازیابی پارامترها
        var iterations = int.Parse(parts[0]);
        var algorithm = new HashAlgorithmName(parts[1]);
        var key = Convert.FromBase64String(parts[2]);
        var salt = Convert.FromBase64String(parts[3]);

        // اجرای مشتق‌سازی مجدد با همان پارامترها
        var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            algorithm,
            HashConfig.KeySize);

        // مقایسه زمان-ثابت برای جلوگیری از حمله‌های تایمینگ
        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}
