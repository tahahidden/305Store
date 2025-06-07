using _305.BuildingBlocks.Utils;
using Serilog;

namespace _305.Application.Base.Response;

/// <summary>
/// کلاس کمکی استاتیک برای ساخت پاسخ‌های استاندارد (<see cref="ResponseDto"/>) در عملیات‌های مختلف.
/// شامل پاسخ‌های موفقیت‌آمیز، خطا، عدم وجود داده، وجود تکراری و ... 
/// </summary>
public static class Responses
{
	private static ResponseDto<T> Create<T>(T? data, bool isSuccess, string? message, int code)
	{
		return new ResponseDto<T>
		{
			data = data,
			is_success = isSuccess,
			message = message,
			response_code = code
		};
	}

	#region Success Responses

	public static ResponseDto<T> Success<T>(T? data = default, string? message = null, int code = ResponseCode.Success)
		=> Create(data, true, message ?? Messages.Success(), code);

	public static ResponseDto<T> ChangeOrDelete<T>(T? data = default, string? message = null, int code = ResponseCode.NoContent)
		=> Create(data, true, message ?? Messages.Success(), code);

	public static ResponseDto<T> Data<T>(T data, string? message = null, int code = ResponseCode.Success)
		=> Create(data, true, message ?? Messages.Success(), code);

	#endregion

	#region Failure Responses

	public static ResponseDto<T> Fail<T>(T? data = default, string? message = null, int code = ResponseCode.BadRequest)
		=> Create(data, false, message ?? Messages.Fail(), code);

	public static ResponseDto<T> ExceptionFail<T>(T? data = default, string? message = null, int code = ResponseCode.InternalServerError)
	{
		Log.Error("خطای سیستمی: {Message}", message ?? Messages.ExceptionFail());
		return Create(data, false, message ?? Messages.ExceptionFail(), code);
	}

	public static ResponseDto<T> Exist<T>(T? data, string? name, string property, string? message = null)
		=> Create(data, false, message ?? Messages.Exist(name, property), ResponseCode.Conflict);

	public static ResponseDto<T> NotFound<T>(T? data, string? name = null, string? message = null)
		=> Create(data, false, message ?? Messages.NotFound(name), ResponseCode.NotFound);

	public static ResponseDto<T> NotValid<T>(T? data, string propName = "آیتم", string? message = null, int code = ResponseCode.BadRequest)
		=> Create(data, false, message ?? Messages.Required(propName), code);

	#endregion
}
