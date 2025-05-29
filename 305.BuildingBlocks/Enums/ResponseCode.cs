using System;
using System.Collections.Generic;
using System.Text;

namespace _305.BuildingBlocks.Enums;
public static class ResponseCode
{
	public const int Success = 200;
	public const int NoContent = 204;
	public const int BadRequest = 400;
	public const int NotFound = 404;
	public const int Conflict = 409;
	public const int InternalServerError = 500;
}
