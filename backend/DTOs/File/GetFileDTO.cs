namespace backend.DTOs.File
{
    public record GetFileDTO(byte[] fileBytes, string contentType, string fullPath, string fileName);
}
