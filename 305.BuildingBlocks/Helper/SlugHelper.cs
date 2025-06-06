using System.Globalization;
using System.Text;
using static System.Text.RegularExpressions.Regex;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// کلاس کمکی برای تولید Slug از یک رشته.
/// Slug رشته‌ای است مناسب برای استفاده در آدرس URL ها (مثلاً: "this-is-a-title").
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// تولید Slug از یک عبارت متنی، با نرمال‌سازی و حذف کاراکترهای غیرمجاز.
    /// پشتیبانی از حروف فارسی و تبدیل آن‌ها به معادل لاتین نیز انجام می‌شود.
    /// </summary>
    /// <param name="phrase">عبارت ورودی</param>
    /// <returns>رشته‌ای بهینه‌شده برای استفاده در URL</returns>
    public static string GenerateSlug(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        // ۱. تبدیل ارقام فارسی به انگلیسی (مثلاً ۱۲۳ → 123)
        phrase = ConvertPersianNumbersToEnglish(phrase);

        // ۲. تبدیل حروف فارسی به معادل انگلیسی ساده (transliteration)
        phrase = ReplacePersianCharacters(phrase);

        // ۳. تبدیل به حروف کوچک و نرمال‌سازی برای حذف لهجه‌ها
        var normalized = phrase.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        // ۴. حذف کاراکترهای ترکیبی (اعراب، لهجه‌ها و ...)
        var sb = new StringBuilder();
        foreach (var c in from c in normalized
                          let category = CharUnicodeInfo.GetUnicodeCategory(c)
                          where category != UnicodeCategory.NonSpacingMark
                          select c)
        {
            sb.Append(c);
        }

        var cleaned = sb.ToString();

        // ۵. حذف هر چیزی که حرف یا عدد یا فاصله نباشد (مثلاً !@#$)
        cleaned = Replace(cleaned, @"[^a-z0-9\s-]", "");

        // ۶. تبدیل فاصله‌ها یا - های متوالی به یک "-"
        cleaned = Replace(cleaned, @"[\s-]+", "-").Trim('-');

        return cleaned;
    }

    /// <summary>
    /// جایگزینی برخی حروف فارسی با معادل لاتین آن‌ها (transliteration)
    /// </summary>
    private static string ReplacePersianCharacters(string input)
    {
        return input
            .Replace("آ", "a")
            .Replace("ا", "a")
            .Replace("ب", "b")
            .Replace("پ", "p")
            .Replace("ت", "t")
            .Replace("ث", "s")
            .Replace("ج", "j")
            .Replace("چ", "ch")
            .Replace("ح", "h")
            .Replace("خ", "kh")
            .Replace("د", "d")
            .Replace("ذ", "z")
            .Replace("ر", "r")
            .Replace("ز", "z")
            .Replace("ژ", "zh")
            .Replace("س", "s")
            .Replace("ش", "sh")
            .Replace("ص", "s")
            .Replace("ض", "z")
            .Replace("ط", "t")
            .Replace("ظ", "z")
            .Replace("ع", "a")
            .Replace("غ", "gh")
            .Replace("ف", "f")
            .Replace("ق", "gh")
            .Replace("ک", "k")
            .Replace("گ", "g")
            .Replace("ل", "l")
            .Replace("م", "m")
            .Replace("ن", "n")
            .Replace("و", "v")
            .Replace("ه", "h")
            .Replace("ی", "y")
            .Replace("ء", "")
            .Replace("ٔ", "")
            .Replace("ى", "y")
            .Replace("ة", "h");
    }

    /// <summary>
    /// تبدیل ارقام فارسی (۱۲۳) به انگلیسی (123)
    /// </summary>
    private static string ConvertPersianNumbersToEnglish(string input)
    {
        var persianDigits = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        for (var i = 0; i < persianDigits.Length; i++)
        {
            input = input.Replace(persianDigits[i], i.ToString()[0]);
        }
        return input;
    }
}
