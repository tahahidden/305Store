using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.IUOW;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.UserRoleFeatures.Handler;

public class DeleteUserRoleCommandHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<DeleteUserRoleCommand, ResponseDto<string>>
{
	private readonly DeleteHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<string>> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
	{
		return await _handler.HandleAsync<UserRole, string>(
			findEntityAsync: () => unitOfWork.UserRoleRepository.FindSingle(x => x.id == request.id),
			onDeleteAsync: entity => unitOfWork.UserRoleRepository.Remove(entity),
			entityName: "ارتباط نقش با کاربر",
			notFoundMessage: "ارتباط نقش با کاربر پیدا نشد",
			successMessage: "ارتباط نقش با کاربر با موفقیت حذف شد",
			cancellationToken: cancellationToken
		);
	}
}