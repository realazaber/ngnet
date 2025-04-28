namespace backend.DTOs.User
{
    public record EditUserDTO(
        string Id,
        string FirstName,
        string LastName, 
        string Email,
        string ProfileImg,
        string Phone);
}
