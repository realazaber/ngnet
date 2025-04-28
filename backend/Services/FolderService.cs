using backend.Data;
using backend.Models.Files;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.DTOs.File;
using backend.DTOs.User;
using Backend.Models.Mappers;
using Backend.DTOs.File;

namespace backend.Services
{
    public class FolderService 
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        
        private readonly IWebHostEnvironment _env;
        private const string UploadsFolder = "wwwroot/Uploads";

        public FolderService(AppDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;            
            _env = env;
        }

        public async Task<List<GetFolderContentsDTO>> GetFolderContents(Guid? folderId)
        {
            List<GetFolderContentsDTO> contents = new List<GetFolderContentsDTO>();

            List<FileEntity> fileEntities;
            List<FolderEntity> folderEntities;

            if (folderId == null)
            {
                fileEntities = await _context.Set<FileEntity>()
                                             .Where(x => x.FolderId == null)
                                             .ToListAsync();

                folderEntities = await _context.Set<FolderEntity>()
                                               .Where(x => x.FolderId == null)
                                               .ToListAsync();
            }
            else
            {
                fileEntities = await _context.Set<FileEntity>()
                                             .Where(x => x.FolderId == folderId)
                                             .ToListAsync();

                folderEntities = await _context.Set<FolderEntity>()
                                               .Where(x => x.FolderId == folderId)
                                               .ToListAsync();
            }

            List<GetFolderContentsDTO> files = fileEntities
                .Select(FileSystemMapper.FileToFolderContentsDTO)
                .ToList();

            List<GetFolderContentsDTO> folders = folderEntities
                .Select(FileSystemMapper.FolderToFolderContentsDTO)
                .ToList();

            contents.AddRange(files);
            contents.AddRange(folders);

            return contents.OrderByDescending(x => x.UpdatedDate).ToList();
        }


        public async Task<IActionResult> CreateFolder(CreateFolderDTO createFolderDTO, ClaimsPrincipal user)
        {
            User currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null)
                return new UnauthorizedResult();

            // Check if folder already exists in the database
            bool folderExistsInDb = await _context.Folders.AnyAsync(f => f.Name == createFolderDTO.Name);
            if (folderExistsInDb)
                return new ConflictObjectResult("A folder with this name already exists in the database.");


            string stringParentFolderPath = UploadsFolder;

            //Creating inside another folder
            if (createFolderDTO.ParentFolderId != null)
            {
                FolderEntity? parentFolder = await _context.Folders.FirstOrDefaultAsync(x => x.Id == createFolderDTO.ParentFolderId);

                if (parentFolder == null)
                {
                    return new BadRequestObjectResult("Parent folder not found.");
                }

                stringParentFolderPath = parentFolder.Path;
            }


            string folderPath = Path.Combine(stringParentFolderPath, createFolderDTO.Name);

            // Log folder path for debugging
            Console.WriteLine($"Checking folder existence: {folderPath}");

            // Check if the folder already exists in the file system
            if (Directory.Exists(folderPath))
                return new ConflictObjectResult("A folder with this name already exists in the file system.");

            Directory.CreateDirectory(folderPath);

            List<string> currentUserRoles = (List<string>)await _userManager.GetRolesAsync(currentUser);
            RetriveUserDTO curentUserDTO = UserMapper.ConvertToRetrieveDTO(currentUser, currentUserRoles);
            
            FolderEntity folder = new FolderEntity
            {
                Id = Guid.NewGuid(),
                Name = createFolderDTO.Name,
                Path = folderPath,
                Description = createFolderDTO.Description,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                FolderId = createFolderDTO.ParentFolderId,
                CreatorId = new Guid(currentUser.Id),
                AuthorizedUsers = new List<RetriveUserDTO> { }
            };

            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            return new OkObjectResult(folder);
        }


        public async Task<IActionResult> EditFolder(Guid folderId, string newName, List<string> userIds, ClaimsPrincipal user)
        {
            var folder = await _context.Folders.Include(f => f.AuthorizedUsers).FirstOrDefaultAsync(f => f.Id == folderId);
            if (folder == null)
                return new NotFoundObjectResult("Folder not found.");

            User currentUser = await _userManager.GetUserAsync(user);
            List<string> currentUserRoles = (List<string>)await _userManager.GetRolesAsync(currentUser);
            RetriveUserDTO currentUserDTO = UserMapper.ConvertToRetrieveDTO(currentUser, currentUserRoles);
            if (currentUser == null || !folder.AuthorizedUsers.Contains(currentUserDTO))
                return new ForbidResult();

            var oldFolderPath = Path.Combine(UploadsFolder, folder.Name);
            var newFolderPath = Path.Combine(UploadsFolder, newName);

            if (Directory.Exists(oldFolderPath) && !Directory.Exists(newFolderPath))
                Directory.Move(oldFolderPath, newFolderPath);

            folder.Name = newName;
            folder.UpdatedDate = DateTime.UtcNow;

            List<User> newUsers = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            List<RetriveUserDTO> newUsersDTOs = new List<RetriveUserDTO>();
            foreach (User loopUser in newUsers)
            {
                List<string> userRoles = (List<string>)await _userManager.GetRolesAsync(loopUser);
                newUsersDTOs.Add(UserMapper.ConvertToRetrieveDTO(loopUser, userRoles));
            }
            folder.AuthorizedUsers.AddRange(newUsersDTOs);

            await _context.SaveChangesAsync();

            return new OkObjectResult(folder);
        }

        public async Task<IActionResult> DeleteFolder(Guid folderId, ClaimsPrincipal user)
        {


            if (await _context.Set<FolderEntity>().AnyAsync(x => x.FolderId == folderId) ||
                await _context.Set<FileEntity>().AnyAsync(x => x.FolderId == folderId))
            {
                return new BadRequestObjectResult("Folder must be empty before it can be deleted.");
            }



            FolderEntity folder = await _context.Set<FolderEntity>().FirstOrDefaultAsync(f => f.Id == folderId);
            if (folder == null)
                return new NotFoundObjectResult("Folder not found.");



            

            

            if (Directory.Exists(folder.Path))
                Directory.Delete(folder.Path);

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Folder deleted successfully." });
        }
    }
}
