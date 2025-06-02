using _305.Application.Base.Handler;
using _305.Application.Base.Response;
using _305.Application.Base.Validator;
using _305.Application.Features.BlogFeatures.Command;
using _305.Application.IBaseRepository;
using _305.Application.IUOW;
using _305.BuildingBlocks.Helper;
using _305.Domain.Entity;
using MediatR;

namespace _305.Application.Features.BlogFeatures.Handler;

public class EditBlogCommandHandler : IRequestHandler<EditBlogCommand, ResponseDto<string>>
{
    private readonly EditHandler<EditBlogCommand, Blog> _handler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Blog> _repository;
    public EditBlogCommandHandler(IUnitOfWork unitOfWork, IRepository<Blog> repository)
    {
        _unitOfWork = unitOfWork;
        _handler = new EditHandler<EditBlogCommand, Blog>(unitOfWork, repository);
        _repository = repository;
    }

    public async Task<ResponseDto<string>> Handle(EditBlogCommand request, CancellationToken cancellationToken)
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
            propertyName: "مقاله",
            beforeUpdate: async entity =>
            {
                // داینامیک مدیریت تصویر
                request.image = entity.image;
                if (request.image_file != null && request.image_file.Length > 0)
                {
                    if (!string.IsNullOrEmpty(entity.image))
                        FileHelper.DeleteImage(entity.image);

                    var result = await FileHelper.UploadImage(request.image_file);
                    if (!string.IsNullOrEmpty(result))
                        request.image = result;
                }
            },
            updateEntity: async entity =>
            {
                entity.name = request.name;
                entity.slug = slug;
                entity.updated_at = request.updated_at;
                entity.description = request.description ?? "";
                entity.meta_description = request.meta_description;
                entity.blog_text = request.blog_text;
                entity.estimated_read_time = request.estimated_read_time;
                entity.blog_category_id = request.blog_category_id;
                entity.image = request.image;
                entity.keywords = request.keywords;
                entity.show_blog = request.show_blog;
                return slug;
            },
            cancellationToken: cancellationToken
        );
    }
}

