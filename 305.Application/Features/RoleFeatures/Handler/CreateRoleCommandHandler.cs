using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.RoleFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RoleFeatures.Handler;

public class CreateRoleCommandHandler(IUnitOfWork unitOfWork)
	: IRequestHandler<CreateRoleCommand, ResponseDto<string>>
{
	private readonly CreateHandler _handler = new(unitOfWork);

	public async Task<ResponseDto<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
	{

		var slug = SlugHelper.GenerateSlug(request.name);
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
		   }
		};

		return await _handler.HandleAsync(
		   validations: validations,
		   onCreate: async () =>
		   {
			   var entity = Mapper.Map<CreateRoleCommand, Role>(request);
			   await unitOfWork.RoleRepository.AddAsync(entity);
			   return slug;
		   },
		   successMessage: null,
		   cancellationToken: cancellationToken
	   );
	}
}