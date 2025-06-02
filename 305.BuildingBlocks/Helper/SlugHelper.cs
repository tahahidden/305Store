using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// کلاس کمکی برای تولید Slug از یک عبارت متنی
/// Slug رشته‌ای است مناسب برای استفاده در URL (مثلاً: "this-is-a-title")
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// تولید یک Slug از یک رشته‌ی ورودی (مثلاً تبدیل "سلام دنیا!" به "salam-donya")
    /// </summary>
    /// <param name="phrase">عبارت ورودی</param>
    /// <returns>رشته‌ای قابل استفاده در آدرس URL</returns>
    public static string GenerateSlug(string phrase)
    {
        // اگر رشته خالی یا null باشد، رشته خالی برگردان
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        // تبدیل عبارت به حروف کوچک و نرمال‌سازی آن برای حذف لهجه‌ها
        string normalized = phrase.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        // حذف کاراکترهای غیر پایه (مثل لهجه‌ها، اعراب و...)
        var sb = new StringBuilder();
        foreach (char c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        string cleaned = sb.ToString();

        // حذف هر چیزی که حرف یا عدد یا فاصله نباشد
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", "");

        // تبدیل تمام فاصله‌ها یا خط‌تیره‌های متوالی به یک خط‌تیره و حذف خط‌تیره‌های ابتدا و انتها
        cleaned = Regex.Replace(cleaned, @"[\s-]+", "-").Trim('-');

        return cleaned;
    }
}
