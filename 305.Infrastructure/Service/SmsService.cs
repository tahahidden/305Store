using _305.Application.IService;
using Kavenegar;
using Kavenegar.Models;
using Kavenegar.Models.Enums;

namespace _305.Infrastructure.Service;
public class SmsService : ISmsService
{
    // test api code
    private const string ApiKey = "7762575A6D727561537779314251716C6B2B6C444B6E38316445357A6B3143306D634C42316A7A2F546C4D3D";
    private const string Number = "9982003589";

    public void SendForgotPass(string phone, string pass)
    {
        //send
    }

    public SendResult SendSms(string recipient, string message)
    {
        var api = new KavenegarApi(ApiKey);
        var result = api.Send(Number, recipient, message);

        return result;
    }
    // ارسال کد OTP
    public string SendOtp(string recipient, string token)
    {
        try
        {
            var api = new KavenegarApi(ApiKey);
            // ارسال کد OTP با استفاده از قالب کاوه نگار
            api.VerifyLookup(recipient, token, template: "LightGymLogin", type: VerifyLookupType.Sms);
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
    public string SendBulkSms(List<string> recipients, string message)
    {
        try
        {
            var api = new KavenegarApi(ApiKey);
            var senders = new List<string> { Number };
            var messages = new List<string> { message };
            // Send the messages
            var results = api.SendArray(senders, recipients, messages);

            // Build the response for logging or display
            var response = "Bulk SMS Results:\n";
            foreach (var result in results)
            {
                response += $"Recipient: {result.Receptor}, Status: {result.StatusText}, Message ID: {result.Messageid}\n";
            }

            return response;
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
