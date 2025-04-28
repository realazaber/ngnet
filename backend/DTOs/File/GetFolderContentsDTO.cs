namespace Backend.DTOs.File
{
    public record GetFolderContentsDTO(Guid Id, 
                                string Name, 
                                string Description, 
                                string Path, 
                                DateTime CreatedDate, 
                                DateTime UpdatedDate,
                                bool IsFolder,
                                Guid? ParentFolderId);
}
