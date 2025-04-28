namespace Backend.DTOs.File
{
    public record GetFileSystemEntityDTO(
        Guid Id,
        string Name, 
        string Description,
        Guid CreatorId,
        DateTime CreatedDate,
        DateTime UpdatedDate);
    
}
