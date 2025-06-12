using _305.Application.IService;
using _305.BuildingBlocks.Configurations;
using Kavenegar;
using Kavenegar.Models;
using Kavenegar.Models.Enums;
using System.Text;

namespace _305.Infrastructure.Service
{
    /// <summary>
    /// سرویس ارسال پیامک با استفاده از سرویس کاوه نگار (Kavenegar).
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly KavenegarApi _api;

        /// <summary>
        /// سازنده سرویس، اینجا اتصال به API کاوه نگار ایجاد می‌شود.
        /// </summary>
        public SmsService()
        {
            _api = new KavenegarApi(SmsConfig.ApiKey);
        }

        /// <summary>
        /// ارسال پیامک بازیابی رمز عبور (متد فعلاً خالی است، جای توسعه دارد).
        /// </summary>
        public void SendForgotPass(string phone, string pass)
        {
            // TODO: پیاده‌سازی ارسال پیام بازیابی رمز عبور
        }

        /// <summary>
        /// ارسال یک پیامک معمولی به یک شماره مشخص.
        /// </summary>
        /// <param name="recipient">شماره دریافت کننده</param>
        /// <param name="message">متن پیامک</param>
        /// <returns>نتیجه ارسال پیامک</returns>
        public SendResult SendSms(string recipient, string message)
        {
            return _api.Send(SmsConfig.SenderNumber, recipient, message);
        }

        /// <summary>
        /// ارسال کد OTP با استفاده از قالب آماده کاوه نگار.
        /// </summary>
        /// <param name="recipient">شماره دریافت کننده</param>
        /// <param name="token">کد OTP</param>
        /// <returns>پیغام نتیجه ارسال</returns>
        public string SendOtp(string recipient, string token)
        {
            try
            {
                _api.VerifyLookup(recipient, token, template: "LightGymLogin", type: VerifyLookupType.Sms);
                return "OTP sent successfully.";
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                return $"API Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}";
            }
        }

        /// <summary>
        /// ارسال پیامک به چندین شماره به صورت Bulk.
        /// </summary>
        /// <param name="recipients">لیست شماره‌های دریافت کننده</param>
        /// <param name="message">متن پیامک</param>
        /// <returns>خروجی شامل وضعیت ارسال هر پیام</returns>
        public string SendBulkSms(List<string> recipients, string message)
        {
            try
            {
                var senders = new List<string> { SmsConfig.SenderNumber };
                var messages = new List<string> { message };

                var results = _api.SendArray(senders, recipients, messages);

                var response = new StringBuilder("Bulk SMS Results:\n");
                foreach (var result in results)
                {
                    response.AppendLine($"Recipient: {result.Receptor}, Status: {result.StatusText}, Message ID: {result.Messageid}");
                }

                return response.ToString();
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                return $"API Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"General Error: {ex.Message}";
            }
        }
    }
}
