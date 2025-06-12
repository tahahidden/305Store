using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.RoleFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RoleFeatures.Handler;
public class DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRoleCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<Role, string>(
            findEntityAsync: () => unitOfWork.RoleRepository.FindSingle(x => x.id == request.id),
            onDeleteAsync: entity => unitOfWork.RoleRepository.Remove(entity),
            entityName: "نقش",
            notFoundMessage: "نقش پیدا نشد",
            successMessage: "نقش با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}