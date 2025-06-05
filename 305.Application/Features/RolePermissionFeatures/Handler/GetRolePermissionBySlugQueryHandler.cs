using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Query;
using _305.Application.Features.UserRoleFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using _305.Application.Features.RolePermissionFeatures.Query;
using _305.Application.Features.RolePermissionFeatures.Response;

namespace _305.Application.Features.RolePermissionFeatures.Handler;
public class GetRolePermissionBySlugQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetRolePermissionBySlugQuery, ResponseDto<RolePermissionResponse>>
{
	private readonly GetBySlugHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<RolePermissionResponse>> Handle(GetRolePermissionBySlugQuery request, CancellationToken cancellationToken)
	{
		return await _handler.Handle<RolePermission, RolePermissionResponse>(
			async uow => await uow.RolePermissionRepository.FindSingle(x => x.slug == request.slug),
			"ارتباط نقش با دسترسی",
			null
		);
	}
}

