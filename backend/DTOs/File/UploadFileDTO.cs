namespace backend.DTOs.File
{
    public record UploadFileDTO(
        string Name,
        string Path,        
        string Description,
        IFormFile File,
        Guid? FolderId);
}
