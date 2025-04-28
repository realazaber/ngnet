using backend.DTOs.User;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models.Files
{
    public class FileSystemEntity : AuthEntity
    {
        public string Path { get; set; }

        public Guid? FolderId { get; set; }

        public string Description { get; set; }

        public List<RetriveUserDTO>? AuthorizedUsers { get; set; }

        public List<IdentityRole>? AuthorizedRoles { get; set; }
    }
}
