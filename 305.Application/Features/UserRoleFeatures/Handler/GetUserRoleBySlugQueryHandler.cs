using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Features.UserRoleFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.UserRoleFeatures.Handler;

public class GetUserRoleBySlugQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserRoleBySlugQuery, ResponseDto<UserRoleResponse>>
{
    private readonly GetBySlugHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<UserRoleResponse>> Handle(GetUserRoleBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _handler.Handle<UserRole, UserRoleResponse>(
            async uow => await uow.UserRoleRepository.FindSingle(x => x.slug == request.slug),
            "ارتباط نقش با کاربر",
            null
        );
    }
}
