using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.ArchiveSettingsDTOs;
using Hoshi.Models.DashboardModels;

namespace Hoshi.Controllers.DashboardControllers.ArchiveSettingsControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ArchiveSettingsController : GenericController<
        HoshiDbContext, 
        ArchiveSettings, 
        ArchiveSettingsGetDTO, 
        ArchiveSettingsPostDTO, 
        ArchiveSettingsPutDTO>
    {
        public ArchiveSettingsController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                ArchiveSettings, 
                ArchiveSettingsGetDTO, 
                ArchiveSettingsPostDTO, 
                ArchiveSettingsPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
        }

        [NonAction]
        public override Task<IActionResult> Add(ArchiveSettingsPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<ArchiveSettingsPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override Task<IActionResult> GetById(int id)
        {
            return base.GetById(id);
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
