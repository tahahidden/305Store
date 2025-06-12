namespace _305.BuildingBlocks.IService;

using Microsoft.AspNetCore.Http;

public interface IFileManager
{
    Task<string> UploadImageAsync(IFormFile file, HttpRequest request, string folderName = _305.BuildingBlocks.Constants.FileDefaults.DefaultFolderName);
    void DeleteImageFile(string imageUrl);
}

