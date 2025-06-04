using System;
using System.Collections.Generic;
using System.Text;
using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.RoleFeatures.Command;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.RoleFeatures.Handler;
public class EditRoleCommandHandler(IUnitOfWork unitOfWork, IRepository<Role> repository)
	: IRequestHandler<EditRoleCommand, ResponseDto<string>>
{
	private readonly EditHandler<EditRoleCommand, Role> _handler = new(unitOfWork, repository);

	public async Task<ResponseDto<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
	{
		var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

		var validations = new List<ValidationItem>
		{
			new ()
			{
				Rule = async () => await repository.ExistsAsync(x => x.name == request.name && x.id != request.id),
				Value = "نام"
			},
			new ()
			{
				Rule = async () => await repository.ExistsAsync(x => x.slug == slug && x.id != request.id),
				Value = "نامک"
			}
		};

		return await _handler.HandleAsync(
			id: request.id,
			validations: validations,
			updateEntity:  entity =>
			{
				entity.name = request.name;
				entity.slug = slug;
				entity.updated_at = request.updated_at;
				return Task.FromResult(slug);
			},

			propertyName: "دسته‌بندی",
			cancellationToken: cancellationToken
		);
	}
}
