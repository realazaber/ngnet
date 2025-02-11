using backend.Data;
using backend.DTOs.File;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Services
{
    public class FileService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;
        private const string UploadsFolder = "wwwroot/Uploads";

        public FileService(AppDbContext context, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task<GetFileDTO> GetFile(string fullPath)
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            string contentType = "application/octet-stream";

            return new GetFileDTO(fileBytes, contentType, fullPath);
        }

        public async Task<FileEntity> UploadFile(IFormFile file, FileType type, ClaimsPrincipal User)
        {
            Guid uniqueFileId = Guid.NewGuid();
            // Generate a unique filename
            var fileName = $"{uniqueFileId}_{file.FileName}";
            var filePath = Path.Combine(UploadsFolder, fileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            User currentUser = await _userManager.GetUserAsync(User);

            FileEntity fileEntity = null;

            if (currentUser != null)
            {
                // Save file metadata to database
                fileEntity = new FileEntity
                {
                    Id = uniqueFileId,
                    Name = file.FileName,
                    Path = filePath,
                    Type = type,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    userId = new Guid(currentUser.Id)
                };

                _context.Files.Add(fileEntity);
                await _context.SaveChangesAsync();
            }

            return fileEntity;
        }

        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var fileEntity = await _context.Files.FindAsync(fileId);
            if (fileEntity == null)
            {
                return new NotFoundObjectResult("File not found in database.");
            }

            string fullPath = Path.Combine(fileEntity.Path);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _context.Files.Remove(fileEntity);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "File deleted successfully." });
        }

    }
}
