using System.Security.Cryptography;

namespace _305.BuildingBlocks.Configurations;
internal static class HashConfig
{
    // اندازه نمک (Salt) به بایت - ۱۶ بایت معادل ۱۲۸ بیت
    internal const int SaltSize = 16;

    // اندازه کلید (Hash) به بایت - ۳۲ بایت معادل ۲۵۶ بیت
    internal const int KeySize = 32;

    // تعداد پیش‌فرض تکرار الگوریتم
    internal const int DefaultIterations = 100_000;

    // الگوریتم هش پیش‌فرض (SHA512)
    internal static readonly HashAlgorithmName DefaultAlgorithm = HashAlgorithmName.SHA512;
}
