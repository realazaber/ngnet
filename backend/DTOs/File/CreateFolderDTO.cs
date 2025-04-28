namespace backend.DTOs.File
{
    public record CreateFolderDTO(string Name, string Description, Guid? ParentFolderId);    
}
