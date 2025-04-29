using backend.DTOs.File;
using backend.Services;
using Backend.DTOs.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Authorize(Roles = "Dms")]
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly FolderService _folderService;

        public FolderController(FolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFolderContents([FromQuery] Guid? folderId)
        {
            List<GetFolderContentsDTO> contents = await _folderService.GetFolderContents(folderId);

            return Ok(contents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDTO createFolderDTO)
        {
            return await _folderService.CreateFolder(createFolderDTO, User);
        }

        [HttpPut]
        public async Task<IActionResult> EditFolder([FromQuery] Guid folderId, [FromBody] EditFolderDTO dto)
        {
            return await _folderService.EditFolder(folderId, dto.NewName, dto.UserIds, User);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFolder([FromQuery] Guid folderId)
        {
            return await _folderService.DeleteFolder(folderId, User);
        }
    }
}
