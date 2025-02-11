using backend.DTOs.File;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {                       
        private const string UploadsFolder = "wwwroot/Uploads";

        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {                        
            _fileService = fileService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFile([FromQuery] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest("File path is required.");

            string fullPath = Path.Combine(UploadsFolder, filePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("File not found.");

            GetFileDTO result = await _fileService.GetFile(fullPath);
            
            return File(result.fileBytes, result.contentType, Path.GetFileName(result.fullPath));
        }

        
        [HttpPost, DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> PostFile(IFormFile file, [FromForm] FileType type)
        {
           if (file == null || file.Length == 0)
           {
               return BadRequest("No file uploaded.");
           }

           string uploadsFolderPath = Path.Combine(UploadsFolder);


           // Ensure directory exists
           if (!Directory.Exists(uploadsFolderPath))
           {
               Directory.CreateDirectory(uploadsFolderPath);
           }

            FileEntity fileEntity = await _fileService.UploadFile(file, type, User);

           return Ok(new { Message = "File uploaded successfully", fileEntity });                        
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromQuery] Guid fileId)
        {
            return await _fileService.DeleteFile(fileId);
        }
    }
}
