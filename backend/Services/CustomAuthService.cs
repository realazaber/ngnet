using backend.Data;
using backend.DTOs.User;
using backend.Models;
using backend.Utils;
using Backend.DTOs.Roles;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class CustomAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CustomAuthService(AppDbContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task CreateUser(CreateUserDTO createUserDTO, string creatorEmail)
        {

            User creator = await _userManager.FindByNameAsync(creatorEmail);
            if (creator == null)
            {
                PrintLogger.PrintLog("Creator does not exist", LogLevels.Error);
                return ;
            }

            Guid creatorId = new Guid(creator.Id);
            User user = new User
            {
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Email = createUserDTO.Email,
                UserName = createUserDTO.Email,                
                ProfileImg = createUserDTO.ProfileImg,
                CreatorId = creatorId
            };


            if (createUserDTO.Role != null)
            {

                // Ensure role exists
                if (!await _roleManager.RoleExistsAsync(createUserDTO.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(createUserDTO.Role));
                }
            }


            // Attempt to create the user
            var createUserResult = await _userManager.CreateAsync(user, createUserDTO.Password);

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    PrintLogger.PrintLog($"User creation failed: {error.Description}", LogLevels.Error);
                }
                return;
            }

            // Add user to the role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, createUserDTO.Role);

            if (!addToRoleResult.Succeeded)
            {
                foreach (var error in addToRoleResult.Errors)
                {
                    PrintLogger.PrintLog($"Adding to role failed: {error.Description}", LogLevels.Error);
                }
            }
        }
     
        public async Task DeleteUser(User user)
        {
            await _userManager.DeleteAsync(user);
        }
    }
}
