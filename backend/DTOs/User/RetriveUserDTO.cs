using Microsoft.AspNetCore.Identity;

namespace backend.DTOs.User
{
    public record RetriveUserDTO(
        Guid Id,
        string FirstName,
        string LastName,
        string ProfileImg,
        string Email,
        string Username,
        string PhoneNumber,
        Guid? CreatorId,
        IList<string> Roles);
    
}
