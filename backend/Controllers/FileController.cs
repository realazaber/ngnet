using backend.DTOs.File;
using backend.Models.Files;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize(Roles = "Dms")]
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;
        private readonly FolderService _folderService;

        public FileController(FileService fileService, FolderService folderService)
        {
            _fileService = fileService;
            _folderService = folderService;
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string description, [FromForm] Guid? folderId)
        {
            var result = await _fileService.UploadFile(file, User, description, folderId);
            if (result == null)
                return BadRequest("File upload failed.");

            return Ok(result);
        }

        [HttpPost("upload/multiple")]
        public async Task<IActionResult> UploadMultipleFiles([FromBody] List<UploadFileDTO> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files provided.");

            var uploadedFiles = new List<FileEntity>();
            

            foreach (UploadFileDTO file in files)
            {
                if (file.FolderId == null)
                {
                    var result = await _fileService.UploadFile(file.File, User, file.Description, file.FolderId);
                    if (result != null)
                    {
                        uploadedFiles.Add(result);
                    }
                }
            }

            return Ok(uploadedFiles);
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            GetFileDTO fileResult = await _fileService.DownloadFile(fileId.ToString());
            if (fileResult == null)
                return NotFound("File not found.");

            return File(fileResult.fileBytes, fileResult.contentType, fileResult.fileName);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            return await _fileService.DeleteFile(fileId);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveFile([FromQuery] Guid fileId, [FromQuery] Guid newFolderId)
        {
            var result = await _fileService.MoveFile(fileId, newFolderId, User);
            if (result == null)
                return BadRequest("File move failed.");

            return Ok(result);
        }
    }
}
