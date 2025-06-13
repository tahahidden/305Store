using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace _305.BuildingBlocks.Utils;

/// <summary>
/// کلاس استاتیک شامل متدهای افزونه‌ای عمومی برای عملیات تاریخ، رشته و enum.
/// </summary>
public static class ExtensionMethods
{
    private static readonly PersianCalendar PersianCalendar = new();
    private static readonly string[] PersianMonthNames =
    [
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    ];

    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی با فرمت کامل: yyyy/MM/dd HH:mm:ss
    /// </summary>
    public static string ToSolar(this DateTime val)
    {
        return
            $"{PersianCalendar.GetYear(val):0000}/{PersianCalendar.GetMonth(val):00}/{PersianCalendar.GetDayOfMonth(val):00} {val.Hour:00}:{val.Minute:00}:{val.Second:00}";
    }

    /// <summary>
    /// نمایش تاریخ شمسی با نام ماه فارسی: "yyyy ماه dd"
    /// </summary>
    public static string ToSolarString(this DateTime val)
    {
        var monthIndex = PersianCalendar.GetMonth(val) - 1;
        return
            $"{PersianCalendar.GetYear(val):0000} {PersianMonthNames[monthIndex]} {PersianCalendar.GetDayOfMonth(val):00}";
    }

    /// <summary>
    /// دریافت فقط سال شمسی از تاریخ
    /// </summary>
    public static string GetYearSolar(this DateTime val)
    {
        return PersianCalendar.GetYear(val).ToString();
    }

    /// <summary>
    /// دریافت فقط نام ماه شمسی از تاریخ
    /// </summary>
    public static string GetMonthSolar(this DateTime val)
    {
        return PersianMonthNames[PersianCalendar.GetMonth(val) - 1];
    }

    /// <summary>
    /// تبدیل کد وضعیت پرداخت به متن فارسی معادل آن
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
    /// برش رشته تا طول مشخص و اضافه کردن "..." اگر طول بیش از حد باشد
    /// </summary>
    /// <param name="input">رشته ورودی</param>
    /// <param name="length">حداکثر طول مجاز</param>
    /// <returns>رشته کوتاه‌شده یا اصلی</returns>
    public static string Truncate(this string input, int length)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= length)
            return input;

        return $"{input[..length]}...";
    }

    /// <summary>
    /// دریافت مقدار قابل نمایش (Display یا Description) از مقدار enum
    /// </summary>
    /// <param name="value">مقدار enum</param>
    /// <returns>متن نمایشی یا مقدار enum</returns>
    public static string? GetEnumDisplayName(this Enum? value)
    {
        if (value == null) return null;

        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        // اولویت: DisplayAttribute
        var displayAttr = field.GetCustomAttribute<DisplayAttribute>();
        if (displayAttr != null) return displayAttr.Name;

        // سپس: DescriptionAttribute
        var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
        return descAttr != null ? descAttr.Description : value.ToString();
    }

    /// <summary>
    /// اصلاح تاریخ شمسی اشتباه و تبدیل آن به میلادی معتبر (در صورت نیاز)
    /// </summary>
    /// <remarks>
    /// اگر تاریخ میلادی وارد شده به صورت مستقیم قابل تشخیص نباشد، به درستی با PersianCalendar تبدیل می‌شود.
    /// </remarks>
    /// <param name="date">تاریخ ورودی</param>
    /// <returns>تاریخ معتبر به میلادی</returns>
    public static DateTime ChangeDateToAd(this DateTime date)
    {
        // تلاش برای شناسایی صحت فرمت تاریخ
        var isValidDate = DateTime.TryParseExact(
            date.ToString("yyyy-MM-dd"),
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);

        if (!isValidDate)
        {
            // تبدیل دستی به میلادی از روی تقویم شمسی
            date = PersianCalendar.ToDateTime(
                date.Year, date.Month, date.Day,
                date.Hour, date.Minute, date.Second, date.Millisecond);
        }

        return date;
    }
}
