namespace backend.DTOs.User
{
    public record LoginDTO
    (
        string? Username,
        string? Email,
        string Password
    );
}
