using MediatR;

namespace _305.Application.Features.AdminAuthFeatures.Command;

public class AdminLogoutCommand : IRequest<ResponseDto<string>>
{
}
