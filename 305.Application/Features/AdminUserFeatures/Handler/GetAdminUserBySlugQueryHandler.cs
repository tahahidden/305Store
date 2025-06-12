using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.AdminUserFeatures.Query;
using _305.Application.Features.AdminUserFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.AdminUserFeatures.Handler;

public class GetAdminUserBySlugQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAdminUserBySlugQuery, ResponseDto<AdminUserResponse>>
{
    private readonly GetBySlugHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<AdminUserResponse>> Handle(GetAdminUserBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _handler.Handle<User, AdminUserResponse>(
            async uow => await uow.UserRepository.FindSingle(x => x.slug == request.slug),
            "کاربر ادمین",
            null
        );
    }
}