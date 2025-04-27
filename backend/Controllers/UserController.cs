using backend.DTOs.User;
using backend.Models;
using Backend.Models.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) { 
            _userManager = userManager;
            _roleManager = roleManager;            
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser() {
            
            User currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null) {
                return BadRequest("Error getting user");
            }

            RetriveUserDTO retriveUserDTO = UserMapper.ConvertToRetrieveDTO(currentUser, await _userManager.GetRolesAsync(currentUser));

            return Ok(retriveUserDTO);
        }

        [HttpGet("view")]
        [Authorize(Roles = "Admin, ManageUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] int pageSize = 15, [FromQuery] int pageNum = 1)
        {
            List<User> users = await _userManager.Users.ToListAsync();

            List<RetriveUserDTO> filteredUsers = new List<RetriveUserDTO>();

            if (User.IsInRole("Admin"))
            {
                foreach (User user in users)
                {                    
                    filteredUsers.Add(UserMapper.ConvertToRetrieveDTO(user, await _userManager.GetRolesAsync(user)));
                }
            }
            else if (User.IsInRole("ManageUsers"))
            {
                // Manually filter users in a loop because async calls are not supported in LINQ queries

                foreach (User user in users)
                {
                    bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    bool isManager = await _userManager.IsInRoleAsync(user, "ManageUsers");

                    if (!isAdmin && !isManager)
                    {                                                
                        filteredUsers.Add(UserMapper.ConvertToRetrieveDTO(user, await _userManager.GetRolesAsync(user)));
                    }
                }                 
            }
            
            filteredUsers = filteredUsers.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return Ok(filteredUsers);
        }


        [HttpGet("single")]
        [Authorize(Roles = "Admin, ManageUsers")]
        public async Task<IActionResult> GetUser([FromQuery] string userId)
        {
            if (userId == null)
            {
                return BadRequest("Please input user Id");
            }

            User selectedUser = await _userManager.FindByIdAsync(userId);           
            
            if (selectedUser == null) {
                return BadRequest("Error retrieving user details");
            }

            if (await _userManager.IsInRoleAsync(selectedUser, "ManageUsers") && !User.IsInRole("Admin"))
            {
                return BadRequest("Only Admins can modify users with MangeUsers role");
            }
            
            return Ok(UserMapper.ConvertToRetrieveDTO(selectedUser, await _userManager.GetRolesAsync(selectedUser)));
        }


        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] EditUserDTO editUserDTO)
        {
            bool canEdit = false;
            User user = await _userManager.FindByIdAsync(editUserDTO.Id);

            //Admin can edit everyone
            if (User.IsInRole("Admin"))
            {
                canEdit = true;
            }
            //User is editing themself
            if (user.Email == User.Identity.Name)
            {
                canEdit = true;
            }

            //ManageUsers Role is editing other users with less permissions
            if (User.IsInRole("ManageUsers") && 
                await _userManager.IsInRoleAsync(user, "ManageUsers") == false &&
                await _userManager.IsInRoleAsync(user, "Admin") == false) {
                canEdit = true;
            }

            if (canEdit)
            {
                user.FirstName = editUserDTO.FirstName;
                user.LastName = editUserDTO.LastName;
                user.Email = editUserDTO.Email;
                user.NormalizedEmail = editUserDTO.Email.ToUpper();
                user.UserName = editUserDTO.Email;
                user.NormalizedUserName = editUserDTO.Email.ToUpper();
                user.ProfileImg = editUserDTO.ProfileImg;
                user.PhoneNumber = editUserDTO.Phone;

                IdentityResult result = await _userManager.UpdateAsync(user);

                return Ok(result);
            }
            else
            {
                return BadRequest("You don't have permissions to edit this user.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, ManageUsers")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userId);

                IdentityResult result = await _userManager.DeleteAsync(user);
                return Ok(result);
            }
            catch {
                return BadRequest("Error deleting user");
            }

        }
    }
}
