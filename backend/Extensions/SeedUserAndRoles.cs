using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Extensions
{
    public static class SeedUserAndRoles
    {
        public static async Task CreateUserAndRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();
            

            if (userManager.Users.Count() < 1)
            {
                string[] roleNames = { "Admin", "ManageUsers", "CanMakeFiles", "CanMakeFolders" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                //Add admin user
                User user = new User
                {
                    Id = Guid.Empty.ToString(),
                    FirstName = Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME"),
                    LastName = Environment.GetEnvironmentVariable("ADMIN_LASTNAME"),
                    Email = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                    UserName = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                    ProfileImg = Environment.GetEnvironmentVariable("ADMIN_PROFILEIMG") ?? "",
                };
                string password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "ManageUsers");
                await userManager.AddToRoleAsync(user, "CanMakeFiles");
                await userManager.AddToRoleAsync(user, "CanMakeFolders");
            }
        }
    }
}
