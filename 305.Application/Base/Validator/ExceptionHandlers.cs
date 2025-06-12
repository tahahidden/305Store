using _305.Application.Base.Response;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Core;

namespace _305.Application.Base.Validator;
public static class ExceptionHandlers
{
	public static ResponseDto<TResult> CancellationException<TResult>(ILogger _logger)
	{
		_logger.Warning("عملیات ایجاد لغو شد توسط CancellationToken");
		return Responses.Fail<TResult>(default, "عملیات لغو شد", 499);
	}
	public static ResponseDto<TResult> GeneralException<TResult>(Exception ex, ILogger _logger)
	{
		_logger.Warning(ex, "خطا در عملیات");
		return Responses.Fail<TResult>(default, "عملیات لغو شد", 500);
	}
}
