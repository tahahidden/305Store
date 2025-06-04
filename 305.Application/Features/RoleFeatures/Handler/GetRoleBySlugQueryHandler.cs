using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.RoleFeatures.Query;
using _305.Application.Features.RoleFeatures.Response;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RoleFeatures.Handler;
public class GetRoleBySlugQueryHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<GetRoleBySlugQuery, ResponseDto<RoleResponse>>
{
	private readonly GetBySlugHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<RoleResponse>> Handle(GetRoleBySlugQuery request, CancellationToken cancellationToken)
	{
		return await _handler.Handle<Role, RoleResponse>(
			async uow => await uow.RoleRepository.FindSingle(x => x.slug == request.slug),
			"نقش",
			null
		);
	}
}
