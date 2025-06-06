using Microsoft.AspNetCore.Http;

namespace _305.BuildingBlocks.IService;
public interface IFileService
{
    Task<string> UploadImage(IFormFile file);
    void DeleteImage(string imageUrl);
}
