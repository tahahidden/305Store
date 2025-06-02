using _305.Application.Features.AdminAuthFeatures.Response;
using MediatR;

namespace _305.Application.Features.AdminAuthFeatures.Query;

public class GetUserProfileQuery : IRequest<ResponseDto<UserResponse>>
{
}
