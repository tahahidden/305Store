using System.Globalization;
using System.Text;
using static System.Text.RegularExpressions.Regex;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار تولید Slug از یک متن.
/// مناسب برای استفاده در URL (مثلاً: this-is-a-title).
/// </summary>
public static class SlugHelper
{
	/// <summary>
	/// تولید Slug از یک رشته با پشتیبانی از حروف فارسی.
	/// </summary>
	/// <param name="phrase">عبارت ورودی</param>
	/// <returns>رشته‌ای بهینه‌شده برای URL</returns>
	public static string GenerateSlug(string phrase)
	{
		if (string.IsNullOrWhiteSpace(phrase))
			return string.Empty;

		var result = phrase;

		result = ConvertPersianDigitsToEnglish(result);        // ۱. تبدیل ارقام فارسی
		result = ReplacePersianLetters(result);                // ۲. تبدیل حروف فارسی به لاتین
		result = NormalizeAndRemoveDiacritics(result);         // ۳. نرمال‌سازی و حذف علائم ترکیبی
		result = RemoveInvalidCharacters(result);              // ۴. حذف کاراکترهای غیرمجاز
		result = CollapseWhitespaceAndDashes(result);          // ۵. تبدیل فاصله/خط تکراری به یک "-"

		return result.Trim('-'); // حذف - ابتدایی یا انتهایی
	}

	#region مراحل تولید Slug (به صورت توابع کوچک)

	/// <summary>
	/// تبدیل ارقام فارسی (۱۲۳) به انگلیسی (123)
	/// </summary>
	private static string ConvertPersianDigitsToEnglish(string input)
	{
		var persianDigits = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
		for (var i = 0; i < persianDigits.Length; i++)
		{
			input = input.Replace(persianDigits[i], i.ToString()[0]);
		}
		return input;
	}

	/// <summary>
	/// جایگزینی حروف فارسی با معادل ساده لاتین آن‌ها
	/// </summary>
	private static string ReplacePersianLetters(string input)
	{
		return input
			.Replace("آ", "a").Replace("ا", "a").Replace("ب", "b").Replace("پ", "p")
			.Replace("ت", "t").Replace("ث", "s").Replace("ج", "j").Replace("چ", "ch")
			.Replace("ح", "h").Replace("خ", "kh").Replace("د", "d").Replace("ذ", "z")
			.Replace("ر", "r").Replace("ز", "z").Replace("ژ", "zh").Replace("س", "s")
			.Replace("ش", "sh").Replace("ص", "s").Replace("ض", "z").Replace("ط", "t")
			.Replace("ظ", "z").Replace("ع", "a").Replace("غ", "gh").Replace("ف", "f")
			.Replace("ق", "gh").Replace("ک", "k").Replace("گ", "g").Replace("ل", "l")
			.Replace("م", "m").Replace("ن", "n").Replace("و", "v").Replace("ه", "h")
			.Replace("ی", "y").Replace("ى", "y").Replace("ء", "").Replace("ٔ", "").Replace("ة", "h");
	}

	/// <summary>
	/// نرمال‌سازی و حذف علائم ترکیبی (مثل اعراب، لهجه‌ها و ...)
	/// </summary>
	private static string NormalizeAndRemoveDiacritics(string input)
	{
		var normalized = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
		var sb = new StringBuilder();

		foreach (var c in normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
		{
			sb.Append(c);
		}
		return sb.ToString();
	}

	/// <summary>
	/// حذف کاراکترهای غیرمجاز (غیر از حروف، عدد، فاصله)
	/// </summary>
	private static string RemoveInvalidCharacters(string input)
	{
		return Replace(input, @"[^a-z0-9\s-]", "");
	}

	/// <summary>
	/// تبدیل فاصله‌ها و خط‌های تکراری به یک -
	/// </summary>
	private static string CollapseWhitespaceAndDashes(string input)
	{
		return Replace(input, @"[\s-]+", "-");
	}

	#endregion
}
