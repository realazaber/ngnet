using backend.Models;
using backend.Models.Files;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
       
        public DbSet<DbEvent> Events { get; set; }        

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<FileEntity> Files { get; set; }

        public DbSet<FolderEntity> Folders { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

    }
}
