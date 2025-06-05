using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Application.Features.RolePermissionFeatures.Handler;
public class DeleteRolePermissionCommandHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<DeleteRolePermissionCommand, ResponseDto<string>>
{
	private readonly DeleteHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<string>> Handle(DeleteRolePermissionCommand request, CancellationToken cancellationToken)
	{
		return await _handler.HandleAsync<RolePermission, string>(
			findEntityAsync: () => unitOfWork.RolePermissionRepository.FindSingle(x => x.id == request.id),
			onDeleteAsync: entity => unitOfWork.RolePermissionRepository.Remove(entity),
			entityName: "ارتباط نقش با دسترسی",
			notFoundMessage: "ارتباط نقش با دسترسی پیدا نشد",
			successMessage: "ارتباط نقش با دسترسی با موفقیت حذف شد",
			cancellationToken: cancellationToken
		);
	}
}