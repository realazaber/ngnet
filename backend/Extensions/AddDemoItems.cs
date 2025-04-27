using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Extensions
{
    public static class AddDemoItems
    {
        public static async Task AddDemoUsers(UserManager<User> userManager)
        {
            List<User> demoUsers = new List<User>();

            demoUsers.Add(new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                UserName = "johndoe@gmail.com",
                ProfileImg = "",
            });

            demoUsers.Add(new User
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "janesmith@gmail.com",
                UserName = "janesmith@gmail.com",
                ProfileImg = "",
            });

            demoUsers.Add(new User
            {
                FirstName = "Michael",
                LastName = "Johnson",
                Email = "michaelj@gmail.com",
                UserName = "michaelj@gmail.com",
                ProfileImg = "",
            });

            demoUsers.Add(new User
            {
                FirstName = "Emily",
                LastName = "Brown",
                Email = "emilyb@gmail.com",
                UserName = "emilyb@gmail.com",
                ProfileImg = "",
            });

            foreach (User user in demoUsers)
            {
                await userManager.CreateAsync(user, "Password1234$");
            }

            User sampleManageUser = await userManager.FindByEmailAsync("michaelj@gmail.com");
            await userManager.AddToRoleAsync(sampleManageUser, "ManageUsers");

            User dmsUser = await userManager.FindByEmailAsync("emilyb@gmail.com");
            await userManager.AddToRoleAsync(dmsUser, "Dms");
        }
    }
}
