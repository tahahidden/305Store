using _305.Application.Base.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace _305.WebApi.Base;

/// <summary>
/// کنترلر پایه برای همه کنترلرها با امکانات مشترک مانند هندل کردن خطاها و اجرای درخواست‌ها با MediatR.
/// </summary>
public class BaseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// ساخت پاسخ خطا هنگام نامعتبر بودن مدل ارسالی.
    /// </summary>
    /// <returns>کد 400 با پیام خطای مدل نامعتبر</returns>
    protected IActionResult InvalidModelResponse()
    {
        var errorMessages = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .Where(msg => !string.IsNullOrWhiteSpace(msg));

        var combinedMessage = string.Join(" | ", errorMessages);

        return BadRequest(Responses.Fail<string>(null, combinedMessage));
    }

    /// <summary>
    /// هندل کردن استثناهای غیرمنتظره در سطح کنترلر.
    /// </summary>
    /// <param name="ex">شیء استثنا</param>
    /// <param name="contextMessage">پیام قابل فهم برای کاربر</param>
    /// <returns>کد 400 با پیام خطا</returns>
    protected IActionResult HandleException(Exception ex, string? contextMessage = null)
    {
        var userFriendlyMessage = contextMessage ?? "خطای غیرمنتظره‌ای رخ داده است";

        Log.Error(ex, "❌ {ContextMessage} | {ExceptionMessage}", userFriendlyMessage, ex.Message);

        return BadRequest(Responses.ExceptionFail<object>(null, userFriendlyMessage));
    }

    /// <summary>
    /// اجرای درخواست‌های Query با استفاده از MediatR و هندل خطا.
    /// </summary>
    /// <typeparam name="TResponse">نوع پاسخ</typeparam>
    /// <param name="request">درخواست Query</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>نتیجه اجرای درخواست</returns>
    protected async Task<IActionResult> ExecuteQuery<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// اجرای درخواست‌های Command با اعتبارسنجی مدل، استفاده از MediatR و هندل خطا.
    /// </summary>
    /// <typeparam name="TRequest">نوع درخواست</typeparam>
    /// <typeparam name="TResponse">نوع پاسخ</typeparam>
    /// <param name="request">درخواست Command</param>
    /// <param name="cancellationToken">توکن لغو عملیات</param>
    /// <returns>نتیجه اجرای درخواست</returns>
    protected async Task<IActionResult> ExecuteCommand<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        try
        {
            if (!ModelState.IsValid)
                return InvalidModelResponse();

            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
