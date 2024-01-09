using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
	[Route("file")]
	[Authorize]
	public class FileController : ControllerBase
	{
		[HttpGet]
		[ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
		public ActionResult GetFile([FromQuery] string fileName)
		{
			var rootPath = Directory.GetCurrentDirectory();
			
			var filePath = $"{rootPath}/PrivateFiles/{fileName}";

			bool fileExists = System.IO.File.Exists(filePath);
			if (fileExists)
			{
				var contentProvider = new FileExtensionContentTypeProvider();
				contentProvider.TryGetContentType(filePath, out string contentType);

				var fileContents = System.IO.File.ReadAllBytes(filePath);
				return File(fileContents, contentType, fileName);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public ActionResult Upload([FromForm] IFormFile file)
		{
			var rootPath = Directory.GetCurrentDirectory();
			var fileName = file.FileName;
			var fullPath = $"{rootPath}/PrivateFiles/{fileName}";

			if(file != null && file.Length > 0)
			{
				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					file.CopyTo(stream);
				}
				return Ok();
			}
			return BadRequest();
		}
	}
}
