using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.ArchiveDTOs;
using Hoshi.Models.DashboardModels;
using Hoshi.Repositories.ArchiveService;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.DashboardControllers.ArchiveControllers
{
    [Authorize]
    [NonController]
    //[ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ArchiveController : SoftDeleteGenericController<
        HoshiDbContext, 
        Archive, 
        ArchiveGetDTO, 
        ArchivePostDTO, 
        ArchivePutDTO>
    {
        private readonly IArchiveService archiveService;

        public ArchiveController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Archive, 
                ArchiveGetDTO, 
                ArchivePostDTO, 
                ArchivePutDTO> genericCRUDService,
            IArchiveService archiveService
        ) : base(mapper, genericCRUDService)
        {
            this.archiveService = archiveService;
        }

        public override async Task<IActionResult> Add(ArchivePostDTO postDTO)
        {
            var result = await archiveService.AddArchive(postDTO);
            return StatusCode(result.StatusCode, result);
        }

        public override async Task<IActionResult> Update(ArchivePutDTO putDTO)
        {
            var result = await archiveService.UpdateArchive(putDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
