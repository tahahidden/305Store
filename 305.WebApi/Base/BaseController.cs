using _305.Application.Base.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace _305.WebApi.Base;
public class BaseController : ControllerBase
{
	protected IActionResult InvalidModelResponse()
	{
		var errors = string.Join(" | ", ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage));
		return BadRequest(Responses.Fail<string>(default, errors));
	}

	protected IActionResult HandleException(Exception ex, string? contextMessage = null)
	{
		var message = contextMessage ?? "خطای غیرمنتظره‌ای رخ داده است";

		Log.Error(ex, "❌ {ContextMessage} | {ExceptionMessage}", message, ex.Message);

		// اگر از ResponseDto استفاده می‌کنی:
		return BadRequest(Responses.ExceptionFail<object>(null, message));
	}
}
