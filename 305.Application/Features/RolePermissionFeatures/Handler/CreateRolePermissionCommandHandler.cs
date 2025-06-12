using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.RolePermissionFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RolePermissionFeatures.Handler;
public class CreateRolePermissionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRolePermissionCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
        var validations = new List<ValidationItem>
        {
            new ()
            {
                Rule = async () => await unitOfWork.RolePermissionRepository.ExistsAsync(x => x.name == request.name),
                Value = "نام"
            },
            new ()
            {
                Rule = async () => await unitOfWork.RolePermissionRepository.ExistsAsync(x => x.slug == slug),
                Value = "نامک"
            },
            new ()
            {
                Rule = async () => await unitOfWork.RolePermissionRepository.ExistsAsync(x => x.role_id == request.role_id && x.permission_id == request.permission_id),
                Value = "ارتباط نقش و دسترسی",
            },
            new ()
            {
                Rule = async () => !(await unitOfWork.PermissionRepository.ExistsAsync(x => x.id == request.permission_id)),
                Value = "دسترسی",
                IsExistRole = true
            },
            new ()
            {
                Rule = async () => !(await unitOfWork.RoleRepository.ExistsAsync(x => x.id == request.role_id)),
                Value = "نقش",
                IsExistRole = true
            },
        };
        return await _handler.HandleAsync(
            validations: validations,
            onCreate: async () =>
            {
                var entity = Mapper.Map<CreateRolePermissionCommand, RolePermission>(request);
                await unitOfWork.RolePermissionRepository.AddAsync(entity);
                return slug;
            },
            successMessage: null,
            cancellationToken: cancellationToken
        );
    }
}
