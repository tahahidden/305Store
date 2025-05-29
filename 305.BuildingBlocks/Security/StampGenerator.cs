using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.Text;

namespace _305.BuildingBlocks.Security;

/// <summary>
/// کلاس ایستا برای تولید SecurityStamp تصادفی و امن
/// </summary>
public static class StampGenerator
{
	/// <summary>
	/// ایجاد مهر امنیتی تصادفی با کاراکترهای مجاز قابل انتخاب (پیش‌فرض: حروف و اعداد)
	/// </summary>
	/// <param name="length">طول مهر (باید > 0 باشد)</param>
	/// <param name="allowedChars">کاراکترهای مجاز (پیش‌فرض: AlphanumericCase)</param>
	/// <returns>رشته‌ای امن و تصادفی با حروف بزرگ</returns>
	public static string CreateSecurityStamp(int length, string? allowedChars = null)
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length), "طول باید بیشتر از صفر باشد.");

		var chars = allowedChars ?? AllowedCharacters.AlphanumericCase;

		// تولید رشته تصادفی با استفاده از RandomGenerator و تبدیل به حروف بزرگ
		return RandomGenerator
			.GenerateString(length, chars)
			.ToUpperInvariant();
	}
}
