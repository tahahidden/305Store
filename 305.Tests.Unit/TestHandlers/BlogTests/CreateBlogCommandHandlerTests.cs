using _305.Application.Features.BlogFeatures.Command;
using _305.Application.Features.BlogFeatures.Handler;
using _305.Application.IBaseRepository;
using _305.BuildingBlocks.IService;
using _305.Domain.Entity;
using _305.Tests.Unit.DataProvider;
using _305.Tests.Unit.GenericHandlers;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;

namespace _305.Tests.Unit.TestHandlers.BlogTests;
public class CreateBlogCommandHandlerTests
{

    [Fact]
    public async Task Handle_ShouldCreateBlog_WhenNameAndSlugAndCategoryAreValid()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock
            .Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
            .ReturnsAsync("uploads/test-image.jpg");

        var command = BlogDataProvider.Create(); // این مقدار blog_category_id معتبر داره

        // 🛠️ تعریف اولیه mock دسته‌بندی بلاگ
        var blogCategoryRepoMock = new Mock<IRepository<BlogCategory>>();
        blogCategoryRepoMock
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<BlogCategory, bool>>>()))
            .ReturnsAsync(true); // فرض می‌گیریم دسته‌بندی وجود داره

        await CreateHandlerTestHelper.TestCreateSuccess<
            CreateBlogCommand,
            Blog,
            IRepository<Blog>,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow, fileServiceMock.Object),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: command,
            repoSelector: uow => uow.BlogRepository,
            expectedNameForExistsCheck: command.name,
            setupUowMock: uow =>
            {
                // فقط همین! مقداردهی BlogCategoryRepository، نه BlogRepository
                uow.Setup(x => x.BlogCategoryRepository).Returns(blogCategoryRepoMock.Object);
            }
        );
    }


    [Fact]
    public async Task Handle_ShouldFail_WhenNameIsDuplicate()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await CreateHandlerTestHelper.TestCreateFailure<
            CreateBlogCommand,
            Blog,
            IRepository<Blog>,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow, fileServiceMock.Object),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: BlogDataProvider.Create(),
            repoSelector: uow => uow.BlogRepository,
            setupRepoMock: repo =>
            {
                // name تکراری است => باید true برگرداند تا Valid نباشد
                repo.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(true);
            },
            expectedMessage: null
        );
    }


    [Fact]
    public async Task Handle_ShouldFail_WhenSlugIsDuplicate()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await CreateHandlerTestHelper.TestCreateFailure<
            CreateBlogCommand,
            Blog,
            IRepository<Blog>,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow, fileServiceMock.Object),
            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),
            command: BlogDataProvider.Create(),
            repoSelector: uow => uow.BlogRepository,
            setupRepoMock: repo =>
            {
                repo.SetupSequence(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(false) // برای name → یعنی name تکراری نیست
                    .ReturnsAsync(true); // برای slug → یعنی slug تکراری است
            },
            expectedMessage: null
        );
    }



    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        var command = BlogDataProvider.Create();
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await CreateHandlerTestHelper.TestCreateException<
            CreateBlogCommand,
            Blog,
            IRepository<Blog>,
            CreateBlogCommandHandler>(

            handlerFactory: uow => new CreateBlogCommandHandler(uow, fileServiceMock.Object),

            execute: (handler, cmd, token) => handler.Handle(cmd, token),

            command: command,

            repoSelector: uow => uow.BlogRepository,

            setupRepoMock: repoMock =>
            {
                // نام تکراری نیست
                repoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Blog, bool>>>()))
                    .ReturnsAsync(false);

                // شبیه‌سازی Exception هنگام Add
                repoMock.Setup(r => r.AddAsync(It.IsAny<Blog>()))
                    .ThrowsAsync(new Exception("DB error"));
            }
        );
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenImageFileIsNull()
    {
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(fs => fs.UploadImage(It.IsAny<IFormFile>()))
                       .ReturnsAsync("uploads/test-image.jpg");
        await CreateHandlerTestHelper.TestCreateFailure<
            CreateBlogCommand,
            Blog,
            IRepository<Blog>,
            CreateBlogCommandHandler
        >(
            handlerFactory: uow => new CreateBlogCommandHandler(uow, fileServiceMock.Object),

            execute: (handler, cmd, ct) => handler.Handle(cmd, ct),

            command: new CreateBlogCommand
            {
                name = "Blog Without Image",
                slug = null,
                image_file = null // 🔴 عمداً فایل نذاشتیم
            },

            repoSelector: uow => uow.BlogRepository,

            setupRepoMock: repo =>
            {
                // حتی نیاز نیست چیزی ستاپ کنیم چون کد قبل از رسیدن به validation ها return می‌کنه
            },

            expectedMessage: "تصویر شاخص"
        );
    }
}
