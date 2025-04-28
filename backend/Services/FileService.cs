using backend.Data;
using backend.DTOs.File;
using backend.Models;
using backend.Models.Files;
using Backend.Models.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<GetFileDTO> DownloadFile(string fullPath)
        {
            FileEntity fileEntity = await _context.Set<FileEntity>().FirstAsync(x => x.Path == fullPath);
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            string contentType = "application/octet-stream";

            return new GetFileDTO(fileBytes, contentType, fullPath, fileEntity.Name);
        }
                
        public async Task<FileEntity> UploadFile(IFormFile file, ClaimsPrincipal user, string description, Guid? folderId)
        {
            Guid uniqueFileId = Guid.NewGuid();
            var fileName = $"{uniqueFileId}_{file.FileName}";

            string filePath = UploadsFolder;
            FolderEntity? folder = null;

            if (folderId.HasValue)
            {
                folder = await _context.Folders.FindAsync(folderId.Value);
                if (folder == null)
                    throw new Exception("Folder not found.");

                filePath = Path.Combine(filePath, folder.Name);
            }

            filePath = Path.Combine(filePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null)
                throw new UnauthorizedAccessException();

            var fileEntity = new FileEntity
            {
                Id = uniqueFileId,
                Name = file.FileName,
                Description = description,
                Path = filePath,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatorId = new Guid(currentUser.Id),
                FolderId = folder?.Id
            };

            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync();

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

        public async Task<IActionResult> AddFileToFolder(Guid fileId, Guid folderId)
        {
            FileEntity file = await _context.Files.FindAsync(fileId);
            FolderEntity folder = await _context.Folders.FindAsync(folderId);

            if (file == null || folder == null)
                return new NotFoundObjectResult("File or folder not found.");

            var oldPath = file.Path;
            var newPath = Path.Combine(UploadsFolder, folder.Name, Path.GetFileName(file.Path));

            if (!Directory.Exists(Path.Combine(UploadsFolder, folder.Name)))
                Directory.CreateDirectory(Path.Combine(UploadsFolder, folder.Name));

            if (System.IO.File.Exists(oldPath))
                System.IO.File.Move(oldPath, newPath);

            file.Path = newPath;
            file.FolderId = folderId;
            file.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new OkObjectResult(file);
        }

        public async Task<FileEntity> MoveFile(Guid fileId, Guid? newFolderId, ClaimsPrincipal User)
        {
            FileEntity fileEntity = await _context.Files.FirstOrDefaultAsync(f => f.Id == fileId);
            if (fileEntity == null)
            {
                return null;
            }

            User currentUser = await _userManager.GetUserAsync(User);            
            if (currentUser == null)
            {
                return null;
            }

            if (fileEntity.FolderId.HasValue)
            {
                var oldFolder = await _context.Folders.FindAsync(fileEntity.FolderId);
                if (oldFolder == null || !oldFolder.AuthorizedUsers.Contains(UserMapper.ConvertToRetrieveDTO(currentUser, await _userManager.GetRolesAsync(currentUser))))
                    return null;
            }

            var newFolder = await _context.Folders.Include(f => f.AuthorizedUsers).FirstOrDefaultAsync(f => f.Id == newFolderId);
            if (newFolder == null || !newFolder.AuthorizedUsers.Contains(UserMapper.ConvertToRetrieveDTO(currentUser, await _userManager.GetRolesAsync(currentUser))))
                return null;

            fileEntity.FolderId = newFolderId;

            fileEntity.FolderId = null;
            
            fileEntity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return fileEntity;
        }
    }
}
