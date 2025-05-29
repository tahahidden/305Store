using _305.Application.Base.Response;
using MediatR;

namespace _305.Application.Base.Query;
/// <summary>
/// کوئری عمومی برای دریافت تمامی رکوردها از نوع داده <typeparamref name="TResponse"/>.
/// این کلاس از MediatR برای ارسال درخواست و دریافت پاسخ استفاده می‌کند.
/// </summary>
/// <typeparam name="TResponse">نوع داده‌ای که قرار است لیست آن بازیابی شود.</typeparam>
public class GetAllQuery<TResponse> : IRequest<ResponseDto<List<TResponse>>>
{
}
