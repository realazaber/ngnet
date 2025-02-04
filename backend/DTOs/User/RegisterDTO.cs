namespace backend.DTOs.User
{
    public record RegisterDTO
    (
        string FirstName,
        string LastName,
        string Email,
        string ProfileImg,
        string Password
    );
}
