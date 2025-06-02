using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace _305.BuildingBlocks.Utils;

public static class ExtensionMethods
{
	private static readonly PersianCalendar PersianCalendar = new PersianCalendar();
	private static readonly string[] PersianMonthNames =
	[
		"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
		"مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
	];

	/// <summary>
	/// تبدیل تاریخ میلادی به شمسی با فرمت: yyyy/MM/dd HH:mm:ss
	/// </summary>
	public static string ToShamsi(this DateTime val)
	{
		return string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
			PersianCalendar.GetYear(val),
			PersianCalendar.GetMonth(val),
			PersianCalendar.GetDayOfMonth(val),
			val.Hour, val.Minute, val.Second);
	}

	/// <summary>
	/// نمایش تاریخ شمسی به صورت: "yyyy ماه dd"
	/// </summary>
	public static string ToShamsiString(this DateTime val)
	{
		int monthIndex = PersianCalendar.GetMonth(val) - 1;
		return string.Format("{0:0000} {1} {2:00}",
			PersianCalendar.GetYear(val),
			PersianMonthNames[monthIndex],
			PersianCalendar.GetDayOfMonth(val));
	}

	/// <summary>
	/// گرفتن سال شمسی از تاریخ
	/// </summary>
	public static string GetYearShamsi(this DateTime val)
	{
		return PersianCalendar.GetYear(val).ToString();
	}

	/// <summary>
	/// گرفتن نام ماه شمسی از تاریخ
	/// </summary>
	public static string GetMonthShamsi(this DateTime val)
	{
		return PersianMonthNames[PersianCalendar.GetMonth(val) - 1];
	}

	/// <summary>
	/// گرفتن نام وضعیت پرداخت بر اساس کد عددی
	/// </summary>
	public static string GetStatus(this int status) =>
		status switch
		{
			0 => "وارد درگاه نشد",
			1 => "پرداخت انجام نشده است",
			2 => "پرداخت ناموفق بوده است",
			3 => "خطا رخ داده است",
			4 => "بلوکه شده",
			5 => "برگشت به پرداخت کننده",
			6 => "برگشت خورده سیستمی",
			10 => "در انتظار تایید پرداخت",
			100 => "پرداخت تایید شده است",
			101 => "پرداخت قبلا تایید شده است",
			200 => "به دریافت کننده واریز شد",
			_ => "نا معلوم"
		};

	/// <summary>
	/// برش رشته تا طول مشخص، و اضافه کردن "..." در صورت نیاز
	/// </summary>
	public static string Truncate(this string input, int length)
	{
		if (string.IsNullOrEmpty(input) || input.Length <= length)
			return input;

		return $"{input[..length]}...";
	}

	/// <summary>
	/// گرفتن مقدار Display یا Description از enum
	/// </summary>
	public static string GetEnumDisplayName(this Enum value)
	{
		if (value == null) return null;

		var field = value.GetType().GetField(value.ToString());
		if (field == null) return value.ToString();

		// اولویت با DisplayAttribute
		var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
		if (displayAttr != null) return displayAttr.Name;

		// اگر Display نبود، از Description استفاده می‌کنیم
		var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
		return descAttr != null ? descAttr.Description :
			// در غیر این صورت خود مقدار enum برمی‌گردد
			value.ToString();
	}

	/// <summary>
	/// اصلاح تاریخ برای تبدیل تاریخ شمسی اشتباهی به تاریخ میلادی واقعی (در صورت نیاز)
	/// توجه: این متد ممکن است گیج‌کننده باشد. پیشنهاد: ورودی را مستقیما به شمسی ندهید
	/// </summary>
	public static DateTime FixDate(this DateTime date)
	{
		// اگر به فرمت yyyy-MM-dd قابل تشخیص نیست، آن را با PersianCalendar تبدیل می‌کنیم
		var isValidDate = DateTime.TryParseExact(
			date.ToString("yyyy-MM-dd"),
			"yyyy-MM-dd",
			CultureInfo.InvariantCulture,
			DateTimeStyles.None,
			out _
		);

		if (!isValidDate)
		{
			date = PersianCalendar.ToDateTime(
				date.Year, date.Month, date.Day,
				date.Hour, date.Minute, date.Second, date.Millisecond);
		}

		return date;
	}
}
