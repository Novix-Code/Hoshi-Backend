using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardMdoels.ArchiveDTOs;
using Hoshi.Models.DashboardMdoels;

namespace Hoshi.Controllers.DashboardMdoels.ArchiveControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ArchiveController : SoftDeleteGenericController<
        HoshiDbContext, 
        Archive, 
        ArchiveGetDTO, 
        ArchivePostDTO, 
        ArchivePutDTO>
    {
        public ArchiveController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Archive, 
                ArchiveGetDTO, 
                ArchivePostDTO, 
                ArchivePutDTO> genericCRUDService
        ) : base(mapper, genericCRUDService)
        {
        }
    }
}
