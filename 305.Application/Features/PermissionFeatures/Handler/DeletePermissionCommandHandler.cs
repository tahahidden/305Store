using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.PermissionFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.PermissionFeatures.Handler;

public class DeletePermissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePermissionCommand, ResponseDto<string>>
{
    private readonly DeleteHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        return await _handler.HandleAsync<Permission, string>(
            findEntityAsync: () => unitOfWork.PermissionRepository.FindSingle(x => x.id == request.id),
            onDeleteAsync: entity => unitOfWork.PermissionRepository.Remove(entity),
            entityName: "دسترسی",
            notFoundMessage: "دسترسی پیدا نشد",
            successMessage: "دسترسی با موفقیت حذف شد",
            cancellationToken: cancellationToken
        );
    }
}