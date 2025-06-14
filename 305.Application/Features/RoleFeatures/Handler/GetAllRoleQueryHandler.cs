using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.RoleFeatures.Query;
using _305.Application.Features.RoleFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RoleFeatures.Handler;
public class GetAllRoleQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllRoleQuery, ResponseDto<List<RoleResponse>>>
{
    private readonly GetAllHandler _handler = new();

    public Task<ResponseDto<List<RoleResponse>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        return _handler.HandleAsync<Role, RoleResponse>(
            unitOfWork.RoleRepository.FindListAsync(cancellationToken: cancellationToken)
        );
    }
}