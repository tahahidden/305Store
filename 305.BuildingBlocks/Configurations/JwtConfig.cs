namespace _305.BuildingBlocks.Configurations
{
    public class JwtSetting
    {
        /// <summary>
        /// آیا اعتبارسنجی کلید امضای JWT فعال باشد؟
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; } = true;

        /// <summary>
        /// آیا اعتبارسنجی مدت زمان انقضای توکن فعال باشد؟
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;

        /// <summary>
        /// آیا اعتبارسنجی audience فعال باشد؟
        /// </summary>
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// آیا اعتبارسنجی issuer فعال باشد؟
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// مقدار معتبر برای issuer (فرستنده توکن)
        /// </summary>
        public string ValidIssuer { get; set; } = "https://localhost";

        /// <summary>
        /// مقدار معتبر برای audience (گیرنده توکن)
        /// </summary>
        public string ValidAudience { get; set; } = "https://localhost";

        /// <summary>
        /// کلید امن برای امضای JWT (حداقل 256 بیت)
        /// </summary>
        public string SigningKey { get; set; } = "WQ7+dPhLEHdhdaKNzu!ck-fg86TPhUfd#E&&Qq+=vUtfxJ!@sDfe#u^prXW2&Qhmy33u!@e?5-xb*";

        /// <summary>
        /// مدت اعتبار توکن به دقیقه
        /// </summary>
        public int ExpiryInMinutes { get; set; } = 1024;
    }
}
