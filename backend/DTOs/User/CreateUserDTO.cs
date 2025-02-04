namespace backend.DTOs.User
{
    public record CreateUserDTO(        
        string FirstName,
        string LastName,
        string Email,
        string ProfileImg,
        string Password,
        string Role
        );    
}
