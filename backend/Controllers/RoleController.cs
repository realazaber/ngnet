using Backend.DTOs.Roles;
using backend.DTOs.User;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Authorize(Roles = "ManageUsers,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, 
                                          UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role name is required.");
            }

            IdentityRole identityRole = new IdentityRole(role);
            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { id = identityRole.Id, name = identityRole.Name });
        }


        [HttpPost("addusertorole")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleDTO addUserToRoleDTO)
        {
            if (!await _roleManager.RoleExistsAsync(addUserToRoleDTO.role))
            {
                return BadRequest("Role does not exist.");
            }

            try
            {
                User currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

                if (addUserToRoleDTO.role == "Admin" && await _userManager.IsInRoleAsync(currentUser, "Admin") == false)
                {
                    return BadRequest("Only Admins and add other admins to Admin role");
                }

                User user = await _userManager.FindByIdAsync(addUserToRoleDTO.userId);
                IdentityResult result = await _userManager.AddToRoleAsync(user, addUserToRoleDTO.role);
                return Ok(user + " added to " + addUserToRoleDTO.role);
            }

            catch
            {
                return BadRequest("User does not exist");
            }
        }

        [HttpGet("getroles")]
        public async Task<IActionResult> GetRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync<IdentityRole>();

            return Ok(roles);
        }

        [HttpGet("getroleusers")]
        public async Task<IActionResult> GetRoleUsers([FromQuery] string roleName)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(roleName);
            List<User> users = (List<User>)await _userManager.GetUsersInRoleAsync(roleName);
            List<Guid> userIds = users.Select(x => new Guid(x.Id)).ToList();
            return Ok(new RoleUserCountDTO(new Guid(role.Id), roleName, userIds));

        }

        [HttpDelete("deleterole")]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
        {
            List<User> users = (List<User>)await _userManager.GetUsersInRoleAsync(roleName);
            

            if (users.Count == 0)
            {
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                await _roleManager.DeleteAsync(role);
                return Ok(roleName + " deleted.");
            }
            return BadRequest("There are still users in this role.");
        }

    }
}
