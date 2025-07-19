using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Controllers.GlobalControllers.ComplaintTypeControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ComplaintTypeController : GenericFSPController<
        HoshiDbContext, 
        ComplaintType, 
        ComplaintTypeGetDTO, 
        ComplaintTypePostDTO, 
        ComplaintTypePutDTO>
    {
        public ComplaintTypeController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                ComplaintType, 
                ComplaintTypeGetDTO, 
                ComplaintTypePostDTO, 
                ComplaintTypePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                ComplaintType, 
                ComplaintTypeGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
        }
    }
}
