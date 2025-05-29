using _305.BuildingBlocks.Text;
using System.Security.Cryptography;
using System.Text;

namespace _305.BuildingBlocks.Helper;

/// <summary>
/// ابزار تولید مقادیر تصادفی امن مانند رشته، عدد و کد عددی.
/// از RandomNumberGenerator برای امنیت بیشتر استفاده می‌شود.
/// </summary>
public static class RandomGenerator
{
	// یک نمونه ثابت از RandomNumberGenerator برای استفاده در کل کلاس
	private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

	/// <summary>
	/// تولید رشته‌ای تصادفی با طول مشخص، از بین کاراکترهای مجاز و با پیشوند اختیاری
	/// </summary>
	/// <param name="length">طول کل خروجی نهایی (شامل پیشوند)</param>
	/// <param name="allowedCharacters">کاراکترهای مجاز برای تولید</param>
	/// <param name="prefix">پیشوند اختیاری برای ابتدای رشته</param>
	/// <returns>رشته‌ای تصادفی با فرمت مشخص‌شده</returns>
	public static string GenerateString(int length, string allowedCharacters, string prefix = "")
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length), "طول رشته باید بیشتر از صفر باشد.");

		if (string.IsNullOrWhiteSpace(allowedCharacters))
			throw new ArgumentException("allowedCharacters نباید خالی باشد.", nameof(allowedCharacters));

		// طول واقعی رشته تصادفی (بدون پیشوند)
		int actualLength = length - prefix.Length;
		if (actualLength <= 0)
			throw new ArgumentException("طول رشته باید بزرگتر از طول پیشوند باشد.", nameof(length));

		var result = new StringBuilder(length);
		result.Append(prefix); // افزودن پیشوند به ابتدای خروجی

		var buffer = new byte[sizeof(uint)]; // بافر برای ذخیره بایت‌های تصادفی

		for (int i = 0; i < actualLength; i++)
		{
			Rng.GetBytes(buffer); // پر کردن بافر با بایت‌های تصادفی
			uint num = BitConverter.ToUInt32(buffer, 0); // تبدیل به عدد صحیح
			int index = (int)(num % allowedCharacters.Length); // گرفتن اندیس از کاراکتر مجاز
			result.Append(allowedCharacters[index]); // افزودن کاراکتر انتخاب‌شده
		}

		return result.ToString(); // بازگرداندن رشته نهایی
	}

	/// <summary>
	/// تولید عدد صحیح تصادفی بین min (شامل) و max (خارج از بازه)
	/// </summary>
	/// <param name="max">بیشینه مقدار (خارج از بازه)</param>
	/// <param name="min">کمینه مقدار (شامل بازه)</param>
	/// <returns>عدد صحیح تصادفی بین بازه مشخص‌شده</returns>
	public static int GenerateNumber(int max, int min = 0)
	{
		if (max <= min)
			throw new ArgumentOutOfRangeException(nameof(max), "max باید بزرگتر از min باشد.");

		return RandomNumberGenerator.GetInt32(min, max); // تولید عدد تصادفی امن
	}

	/// <summary>
	/// تولید کد عددی تصادفی با طول مشخص (مثلاً برای OTP، تایید پیامک، کد امنیتی)
	/// </summary>
	/// <param name="length">تعداد رقم کد (پیش‌فرض ۸ رقم)</param>
	/// <returns>کدی عددی به‌صورت رشته</returns>
	public static string GenerateCode(int length = 8)
	{
		return GenerateString(length, AllowedCharacters.Numeric0); // فقط با استفاده از ارقام
	}
}