using _305.Application.Features.BlogCategoryFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Command;
using DataLayer.Base.Handler;
using DataLayer.Base.Response;
using DataLayer.Base.Validator;
using DataLayer.Repository;
using MediatR;

namespace _305.Application.Features.BlogCategoryFeatures.Handler;


public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditCategoryCommand, BlogCategory> _handler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<BlogCategory> _repository;
    public EditCategoryCommandHandler(IUnitOfWork unitOfWork, IRepository<BlogCategory> repository)
    {
        _unitOfWork = unitOfWork;
        // استفاده مستقیم از IUnitOfWork برای دادن Repository به هندلر
        _handler = new EditHandler<EditCategoryCommand, BlogCategory>(_unitOfWork, repository);
        _repository = repository;
    }

    public async Task<ResponseDto<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

		var validations = new List<ValidationItem>
		{
		   new ()
		   {
			   Rule = async () => await _repository.ExistsAsync(x => x.name == request.name && x.id != request.id),
			   Value = "نام"
		   },
		   new ()
		   {
			   Rule = async () => await _repository.ExistsAsync(x => x.slug == slug && x.id != request.id),
			   Value = "نامک"
		   }
		};

		return await _handler.HandleAsync(
            id: request.id,
            validations: validations,
            updateEntity: async entity =>
            {
                entity.name = request.name;
                entity.slug = slug;
                entity.updated_at = request.updated_at;
                entity.description = request.description;
                return slug;
            },

            propertyName: "دسته‌بندی",
            cancellationToken: cancellationToken
        );
    }
}