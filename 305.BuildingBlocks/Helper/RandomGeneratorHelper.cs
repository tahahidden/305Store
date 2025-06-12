using System.Security.Cryptography;
using System.Text;
using _305.BuildingBlocks.Text;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار تولید مقادیر تصادفی امن مانند رشته، عدد و کد عددی.
/// از RandomNumberGenerator برای امنیت بیشتر استفاده می‌شود.
/// </summary>
public static class RandomGenerator
{
	// نمونه‌ای از RandomNumberGenerator برای کل کلاس
	private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

	/// <summary>
	/// تولید یک رشته تصادفی از بین کاراکترهای مجاز داده‌شده، با طول و پیشوند اختیاری.
	/// </summary>
	/// <param name="length">طول کلی خروجی (شامل پیشوند)</param>
	/// <param name="allowedCharacters">کاراکترهای مجاز برای تولید</param>
	/// <param name="prefix">پیشوند اختیاری برای ابتدای رشته</param>
	/// <returns>رشته تصادفی تولیدشده</returns>
	/// <exception cref="ArgumentOutOfRangeException">اگر طول معتبر نباشد</exception>
	/// <exception cref="ArgumentException">اگر کاراکتر مجاز خالی باشد یا پیشوند بیش از طول باشد</exception>
	public static string GenerateString(int length, string allowedCharacters, string prefix = "")
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length), "طول رشته باید بیشتر از صفر باشد.");

		if (string.IsNullOrWhiteSpace(allowedCharacters))
			throw new ArgumentException("لیست کاراکترهای مجاز نباید خالی باشد.", nameof(allowedCharacters));

		var actualLength = length - prefix.Length;
		if (actualLength <= 0)
			throw new ArgumentException("طول رشته باید بزرگتر از طول پیشوند باشد.", nameof(length));

		var result = new StringBuilder(length);
		result.Append(prefix);

		var buffer = new byte[sizeof(uint)];

		for (var i = 0; i < actualLength; i++)
		{
			Rng.GetBytes(buffer);
			var randomNumber = BitConverter.ToUInt32(buffer, 0);
			var index = (int)(randomNumber % allowedCharacters.Length);
			result.Append(allowedCharacters[index]);
		}

		return result.ToString();
	}

	/// <summary>
	/// تولید عدد صحیح تصادفی در بازه [min, max)
	/// </summary>
	/// <param name="max">حد بالا (خارج از بازه)</param>
	/// <param name="min">حد پایین (شامل بازه، پیش‌فرض ۰)</param>
	/// <returns>عدد تصادفی بین min و max</returns>
	/// <exception cref="ArgumentOutOfRangeException">اگر max کوچکتر یا مساوی min باشد</exception>
	public static int GenerateNumber(int max, int min = 0)
	{
		if (max <= min)
			throw new ArgumentOutOfRangeException(nameof(max), "max باید بزرگتر از min باشد.");

		return RandomNumberGenerator.GetInt32(min, max);
	}

	/// <summary>
	/// تولید کد عددی به‌صورت رشته‌ای با تعداد رقم مشخص (مثلاً برای OTP یا تأیید پیامک)
	/// </summary>
	/// <param name="length">تعداد رقم (پیش‌فرض: ۸ رقم)</param>
	/// <returns>کدی عددی به‌صورت رشته</returns>
	public static string GenerateCode(int length = 8)
	{
		return GenerateString(length, AllowedCharacters.Numeric0);
	}
}
