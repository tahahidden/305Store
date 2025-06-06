using _305.Application.Base.Handler;
using _305.Application.Base.Mapper;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogFeatures.Handler;

public class CreateBlogCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    : IRequestHandler<CreateBlogCommand, ResponseDto<string>>
{
    private readonly CreateHandler _handler = new(unitOfWork);

    public async Task<ResponseDto<string>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {

        var slug = request.slug ?? SlugHelper.GenerateSlug(request.name);

        if (request.image_file != null)
        {
            var result = await fileService.UploadImage(request.image_file);
            request.image = result;
        }
        else
        {
            return Responses.NotValid<string>(data: default, propName: "تصویر شاخص");
        }

        var validations = new List<ValidationItem>
        {
           new ()
           {
               Rule = async () => await unitOfWork.BlogRepository.ExistsAsync(x => x.name == request.name),
               Value = "نام"
           },
           new ()
           {
               Rule = async () => await unitOfWork.BlogRepository.ExistsAsync(x => x.slug == slug),
               Value = "نامک"
           }

        };

        return await _handler.HandleAsync(
           validations: validations,
           onCreate: async () =>
           {
               var entity = Mapper.Map<CreateBlogCommand, Blog>(request);
               await unitOfWork.BlogRepository.AddAsync(entity);
               return slug;
           },
           successMessage: null,
           cancellationToken: cancellationToken
       );
    }
}

