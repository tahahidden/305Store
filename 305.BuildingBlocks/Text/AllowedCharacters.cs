namespace _305.BuildingBlocks.Text;
public static class AllowedCharacters
{
    /// <summary>
    /// اعداد 1 تا 9 بدون صفر
    /// </summary>
    public const string Numeric = "123456789";

    /// <summary>
    /// اعداد 0 تا 9 کامل
    /// </summary>
    public const string Numeric0 = "0123456789";

    /// <summary>
    /// اعداد و حروف بزرگ و کوچک (الفبای کامل انگلیسی)
    /// </summary>
    public const string AlphanumericCase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// اعداد و حروف بزرگ فقط
    /// </summary>
    public const string AlphanumericUpper = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// اعداد و حروف کوچک خوانا (بدون i, l, o, q برای جلوگیری از ابهام)
    /// </summary>
    public const string AlphanumericReadable = "0123456789abcdefghkmnprstuvwyz";
}