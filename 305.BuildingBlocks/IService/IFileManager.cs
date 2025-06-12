using _305.BuildingBlocks.Constants;

namespace _305.BuildingBlocks.IService;

using Microsoft.AspNetCore.Http;

public interface IFileManager
{
    Task<string> UploadImageAsync(IFormFile file, HttpRequest request, string folderName = FileDefaults.DefaultFolderName);
    void DeleteImageFile(string imageUrl);
}

