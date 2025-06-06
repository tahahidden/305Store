using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.Service;
public class FileService(IHttpContextAccessor contextAccessor) : IFileService
{
    public void DeleteImage(string imageUrl)
    {
        FileHelper.DeleteImage(imageUrl);
    }

    public async Task<string> UploadImage(IFormFile file)
    {
        return await FileHelper.UploadImage(file, contextAccessor.HttpContext.Request);
    }
}