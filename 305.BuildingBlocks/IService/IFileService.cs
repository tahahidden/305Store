using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.BuildingBlocks.IService;
public interface IFileService
{
	Task<string> UploadImage(IFormFile file);
	void DeleteImage(string imageUrl);
}
