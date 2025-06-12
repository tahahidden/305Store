using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Features.PermissionFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.PermissionFeatures.Handler;

public class GetPermissionBySlugQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPermissionBySlugQuery, ResponseDto<PermissionResponse>>
{
    private readonly GetBySlugHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<PermissionResponse>> Handle(GetPermissionBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _handler.Handle<Permission, PermissionResponse>(
            async uow => await uow.PermissionRepository.FindSingle(x => x.slug == request.slug),
            "ارتباط نقش با کاربر",
            null
        );
    }
}
