using Kavenegar.Models;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace _305.Application.IService;
public interface ISmsService
{
	void SendForgotPass(string Phone, string Pass);

	SendResult SendSms(string recipient, string message);

	string SendOtp(string recipient, string token);

	string SendBulkSms(List<string> recipients, string message);
}