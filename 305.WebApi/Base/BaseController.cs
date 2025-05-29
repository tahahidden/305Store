using _305.Application.Base.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace _305.WebApi.Base;
public class BaseController(IMediator mediator) : ControllerBase
{
	private readonly IMediator _mediator = mediator;
	/// <summary>
	/// ساخت پاسخ خطا برای مدل نامعتبر (InvalidModelState)
	/// </summary>
	protected IActionResult InvalidModelResponse()
	{
		var errorMessages = ModelState.Values
			.SelectMany(v => v.Errors)
			.Select(e => e.ErrorMessage)
			.Where(msg => !string.IsNullOrWhiteSpace(msg));

		var combinedMessage = string.Join(" | ", errorMessages);

		return BadRequest(Responses.Fail<string>(default, combinedMessage));
	}

	/// <summary>
	/// هندل کردن خطاهای غیرمنتظره در سطح کنترلرها
	/// </summary>
	protected IActionResult HandleException(Exception ex, string? contextMessage = null)
	{
		var userFriendlyMessage = contextMessage ?? "خطای غیرمنتظره‌ای رخ داده است";

		Log.Error(ex, "❌ {ContextMessage} | {ExceptionMessage}", userFriendlyMessage, ex.Message);

		return BadRequest(Responses.ExceptionFail<object>(null, userFriendlyMessage));
	}

	protected async Task<IActionResult> ExecuteQuery<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
	{
		try
		{
			var response = await _mediator.Send(request, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}

	protected async Task<IActionResult> ExecuteCommand<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
		where TRequest : IRequest<TResponse>
	{
		try
		{
			if (!ModelState.IsValid)
				return InvalidModelResponse();

			var response = await _mediator.Send(request, cancellationToken);
			return Ok(response);
		}
		catch (Exception ex)
		{
			return HandleException(ex);
		}
	}
}
