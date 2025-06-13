using _305.BuildingBlocks.Constants;

namespace _305.BuildingBlocks.IService;

using Microsoft.AspNetCore.Http;

public interface IFileManager
{
    Task<string> UploadFileAsync(IFormFile file, HttpRequest request, string folderName = FileDefaults.DefaultFolderName);
    void DeleteFile(string imageUrl);
}

