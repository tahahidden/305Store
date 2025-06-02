using Kavenegar.Models;

namespace _305.Application.IService;
public interface ISmsService
{
	void SendForgotPass(string phone, string pass);

	SendResult SendSms(string recipient, string message);

	string SendOtp(string recipient, string token);

	string SendBulkSms(List<string> recipients, string message);
}