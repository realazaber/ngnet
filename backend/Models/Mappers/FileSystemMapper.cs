using backend.Models.Files;
using Backend.DTOs.File;

namespace Backend.Models.Mappers
{
    public static class FileSystemMapper
    {
        public static GetFileSystemEntityDTO FolderToFileSystemDTO(FolderEntity folder) { 
            return new GetFileSystemEntityDTO(folder.Id, 
                                              folder.Name,
                                              folder.Description,
                                              folder.CreatorId,
                                              folder.CreatedDate,
                                              folder.UpdatedDate);
        }

        public static GetFileSystemEntityDTO FileToFileSystemDTO(FileEntity file) {
            return new GetFileSystemEntityDTO(file.Id, 
                                              file.Name, 
                                              file.Description, 
                                              file.CreatorId, 
                                              file.CreatedDate, 
                                              file.UpdatedDate);
        }

        public static GetFolderContentsDTO FileToFolderContentsDTO(FileEntity file) {
            return new GetFolderContentsDTO(file.Id,
                                            file.Name,
                                            file.Description,
                                            file.Path,
                                            file.CreatedDate,
                                            file.UpdatedDate,
                                            false,
                                            file.FolderId ?? null);
        }

        public static GetFolderContentsDTO FolderToFolderContentsDTO(FolderEntity folder) {
            return new GetFolderContentsDTO(folder.Id,
                                            folder.Name,        
                                            folder.Description,
                                            folder.Path,
                                            folder.CreatedDate,
                                            folder.UpdatedDate,
                                            true,
                                            folder.FolderId ?? null);
        }
            
    }
}
