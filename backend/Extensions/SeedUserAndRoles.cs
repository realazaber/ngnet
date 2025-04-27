using backend.Models;
using Backend.Extensions;
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
                string[] roleNames = { "Admin", "ManageUsers", "Dms" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                //Add admin user
                User adminUser = new User
                {
                    Id = Guid.Empty.ToString(),
                    FirstName = Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME"),
                    LastName = Environment.GetEnvironmentVariable("ADMIN_LASTNAME"),
                    Email = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                    UserName = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                    ProfileImg = Environment.GetEnvironmentVariable("ADMIN_PROFILEIMG") ?? "",
                };

                


                string password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
                await userManager.CreateAsync(adminUser, password);



                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddToRoleAsync(adminUser, "ManageUsers");
                await userManager.AddToRoleAsync(adminUser, "Dms");

                if (Environment.GetEnvironmentVariable("DEMO_MODE") == "ENABLED")
                {
                    await AddDemoItems.AddDemoUsers(userManager);
                }

                
            }
        }
    }
}
