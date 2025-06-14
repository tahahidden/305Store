using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Query;
using _305.Application.Features.PermissionFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.PermissionFeatures.Handler;

public class GetAllPermissionQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPermissionQuery, ResponseDto<List<PermissionResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<PermissionResponse>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<Permission, PermissionResponse>(
            unitOfWork.PermissionRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}