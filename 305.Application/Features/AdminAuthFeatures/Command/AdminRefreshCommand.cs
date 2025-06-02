using _305.Application.Base.Response;
using _305.Application.Features.AdminAuthFeatures.Response;
using MediatR;

namespace _305.Application.Features.AdminAuthFeatures.Command;
public class AdminRefreshCommand : IRequest<ResponseDto<LoginResponse>>
{
}