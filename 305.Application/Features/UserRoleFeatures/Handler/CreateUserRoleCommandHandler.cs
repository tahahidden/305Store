using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.UserRoleFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.UserRoleFeatures.Handler;

public class CreateUserRoleCommandHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<CreateUserRoleCommand, ResponseDto<string>>
{
	private readonly CreateHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<string>> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
	{
		var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
		var validations = new List<ValidationItem>
		{
		   new ()
		   {
			   Rule = async () => await unitOfWork.UserRoleRepository.ExistsAsync(x => x.name == request.name),
			   Value = "نام"
		   },
		   new ()
		   {
			   Rule = async () => await unitOfWork.UserRoleRepository.ExistsAsync(x => x.slug == slug),
			   Value = "نامک"
		   },
		   new ()
		   {
			   Rule = async () => await unitOfWork.UserRoleRepository.ExistsAsync(x => x.userid == request.userid && x.roleid == request.roleid),
			   Value = "ارتباط نقش و کاربر",
		   },
		   new ()
		   {
			   Rule = async () => !(await unitOfWork.UserRepository.ExistsAsync(x => x.id == request.userid)),
			   Value = "کاربر",
			   IsExistRole = true
		   },
		   new ()
		   {
			   Rule = async () => !(await unitOfWork.RoleRepository.ExistsAsync(x => x.id == request.roleid)),
			   Value = "نقش",
			   IsExistRole = true
		   },
		};
		return await _handler.HandleAsync(
			validations: validations,
			onCreate: async () =>
			{
				var entity = Mapper.Map<CreateUserRoleCommand, UserRole>(request);
				await unitOfWork.UserRoleRepository.AddAsync(entity);
				return slug;
			},
			successMessage: null,
			cancellationToken: cancellationToken
		);
	}
}