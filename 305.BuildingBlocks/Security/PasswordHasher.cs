using _305.BuildingBlocks.Configurations;
using System.Security.Cryptography;

namespace _305.BuildingBlocks.Security;

/// <summary>
/// کلاس ایستا برای هش کردن رمز عبور و بررسی اعتبار آن با الگوریتم PBKDF2.
/// فرمت هش خروجی: iterations.algorithm.derivedKey.salt (به صورت Base64)
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// هش کردن یک رمز عبور خام با استفاده از الگوریتم PBKDF2.
    /// هش خروجی شامل پارامترهای مورد نیاز برای اعتبارسنجی آینده است.
    /// </summary>
    /// <param name="rawPassword">رمز عبور خام (بدون هش)</param>
    /// <returns>
    /// رشته‌ای از هش شامل: iterations.algorithm.key.salt به صورت base64
    /// </returns>
    public static string Hash(string rawPassword)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException("رمز عبور نمی‌تواند خالی یا null باشد.", nameof(rawPassword));

        // تولید یک نمک تصادفی با اندازه مشخص
        var salt = RandomNumberGenerator.GetBytes(HashConfig.SaltSize);

        // اجرای الگوریتم مشتق‌سازی کلید (PBKDF2) با پارامترهای پیش‌فرض
        var derivedKey = Rfc2898DeriveBytes.Pbkdf2(
            rawPassword,
            salt,
            HashConfig.DefaultIterations,
            HashConfig.DefaultAlgorithm,
            HashConfig.KeySize);

        // تبدیل کلید و نمک به رشته‌های base64 برای ذخیره در دیتابیس یا فایل
        var base64Key = Convert.ToBase64String(derivedKey);
        var base64Salt = Convert.ToBase64String(salt);

        // قالب نهایی: iterations.algorithm.key.salt
        return $"{HashConfig.DefaultIterations}.{HashConfig.DefaultAlgorithm}.{base64Key}.{base64Salt}";
    }

    /// <summary>
    /// بررسی تطابق رمز عبور خام با هش ذخیره‌شده.
    /// با استخراج پارامترها از هش ذخیره‌شده و اجرای دوباره PBKDF2 انجام می‌شود.
    /// </summary>
    /// <param name="storedHash">هش ذخیره‌شده شامل پارامترها</param>
    /// <param name="rawPassword">رمز عبور خام برای بررسی</param>
    /// <returns>اگر رمز عبور صحیح باشد: true، در غیر این صورت: false</returns>
    public static bool Check(string storedHash, string rawPassword)
    {
        // چک اولیه: هر دو مقدار باید معتبر باشند
        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(rawPassword))
            return false;

        // تلاش برای پارس کردن هش ذخیره‌شده
        var isParsed = TryParseHash(storedHash, out var iterations, out var algorithm, out var originalKey, out var salt);
        if (!isParsed)
            throw new FormatException("فرمت هش نامعتبر است. ساختار باید به صورت: iterations.algorithm.key.salt باشد.");

        // تولید مجدد کلید با همان پارامترها و رمز عبور جدید
        var derivedKey = Rfc2898DeriveBytes.Pbkdf2(
            rawPassword,
            salt,
            iterations,
            algorithm,
            HashConfig.KeySize);

        // مقایسه امن کلیدها برای جلوگیری از حمله تایمینگ
        return CryptographicOperations.FixedTimeEquals(originalKey, derivedKey);
    }

    /// <summary>
    /// پارس کردن رشته هش‌شده و استخراج پارامترها
    /// </summary>
    /// <param name="hash">رشته هش‌شده به فرمت: iterations.algorithm.key.salt</param>
    /// <param name="iterations">تعداد تکرار الگوریتم مشتق‌سازی</param>
    /// <param name="algorithm">نام الگوریتم هش (مثلاً: SHA256)</param>
    /// <param name="key">کلید هش‌شده اصلی</param>
    /// <param name="salt">نمک استفاده‌شده</param>
    /// <returns>در صورت موفقیت: true، در غیر این صورت: false</returns>
    private static bool TryParseHash(
        string hash,
        out int iterations,
        out HashAlgorithmName algorithm,
        out byte[] key,
        out byte[] salt)
    {
        iterations = default;
        algorithm = default;
        key = salt = null;

        var parts = hash.Split('.', 4);
        if (parts.Length != 4)
            return false;

        if (!int.TryParse(parts[0], out iterations))
            return false;

        algorithm = new HashAlgorithmName(parts[1]);

        try
        {
            key = Convert.FromBase64String(parts[2]);
            salt = Convert.FromBase64String(parts[3]);
            return true;
        }
        catch
        {
            // در صورت خطا در تبدیل Base64 (برای کلید یا نمک)
            return false;
        }
    }
}