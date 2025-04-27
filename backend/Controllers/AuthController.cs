using backend.DTOs.User;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize(Roles = "ManageUsers,Admin")]    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CustomAuthService _customAuthService;        
           
        public AuthController(CustomAuthService customAuthService)
        {
            _customAuthService = customAuthService;            
         
        }


        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {            
            // Ensure user is authenticated before proceeding
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated");
            }
           
            // Get the current user's Email
            string strCreatorEmail = User.Identity.Name;
          
            if (string.IsNullOrEmpty(strCreatorEmail))
            {
                return BadRequest("Unable to determine the creator ID");
            }

            await _customAuthService.CreateUser(createUserDTO, strCreatorEmail);

            return Ok(new { message = "User created successfully" });
        }        
    }
}
