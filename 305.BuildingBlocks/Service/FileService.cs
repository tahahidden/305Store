using _305.BuildingBlocks.Helper;
using _305.BuildingBlocks.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.BuildingBlocks.Service;
public class FileService : IFileService
{
	private readonly IHttpContextAccessor _contextAccessor;
	public FileService(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public void DeleteImage(string imageUrl)
	{
		FileHelper.DeleteImage(imageUrl);
	}

	public async Task<string> UploadImage(IFormFile file)
	{
		return await FileHelper.UploadImage(file, _contextAccessor.HttpContext.Request);
	}
}