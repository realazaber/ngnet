using Backend.Models.Files;

namespace backend.Models.Files
{
    public class FolderEntity : FileSystemEntity
    {        
        public List<FileEntity>? Files { get; set; }

        public List<FolderEntity>? Folders { get; set; }        
    }
}
