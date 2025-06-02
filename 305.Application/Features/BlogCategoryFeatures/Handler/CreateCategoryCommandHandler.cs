using _305.Application.Features.BlogCategoryFeatures.Command;
using Core.Assistant.Helpers;
using Core.EntityFramework.Models;
using DataLayer.Base.Handler;
using DataLayer.Base.Mapper;
using DataLayer.Base.Response;
using DataLayer.Base.Validator;
using DataLayer.Repository;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace _305.Application.Features.BlogCategoryFeatures.Handler;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _handler = new CreateHandler(unitOfWork);
    }

    public async Task<ResponseDto<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);
        var validations = new List<ValidationItem>
        {
           new ()
           {
               Rule = async () => await _unitOfWork.BlogCategoryRepository.ExistsAsync(x => x.name == request.name),
               Value = "نام"
           },
           new ()
           {
               Rule = async () => await _unitOfWork.BlogCategoryRepository.ExistsAsync(x => x.slug == slug),
               Value = "نامک"
           }

        };
        return await _handler.HandleAsync(
            validations: validations,
            onCreate: async () =>
            {
                var entity = Mapper.Map<CreateCategoryCommand, BlogCategory>(request);
                await _unitOfWork.BlogCategoryRepository.AddAsync(entity);
                return slug;
            },
            createMessage: null,
            cancellationToken: cancellationToken
        );
    }
}