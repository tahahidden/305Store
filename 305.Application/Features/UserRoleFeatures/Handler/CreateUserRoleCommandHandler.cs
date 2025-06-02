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

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, ResponseDto<string>>
{
	private readonly CreateHandler _handler;
	private readonly IUnitOfWork _unitOfWork;

	public CreateUserRoleCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
		_handler = new CreateHandler(unitOfWork);
	}

	public async Task<ResponseDto<string>> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
	{

		var slug = SlugHelper.GenerateSlug(request.user_id.ToString() + request.role_id.ToString());
		var validations = new List<ValidationItem>
		{
		   new ()
		   {
			   Rule = async () => await _unitOfWork.UserRoleRepository.ExistsAsync(x => x.slug == slug),
			   Value = "نامک"
		   }
		};

		return await _handler.HandleAsync(
		   validations: validations,
		   onCreate: async () =>
		   {
			   var entity = Mapper.Map<CreateUserRoleCommand, UserRole>(request);
			   await _unitOfWork.UserRoleRepository.AddAsync(entity);
			   return slug;
		   },
		   successMessage: null,
		   cancellationToken: cancellationToken
	   );
	}
}