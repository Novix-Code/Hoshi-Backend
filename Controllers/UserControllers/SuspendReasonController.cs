using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.SuspendReasonDTOs;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.SuspendReasonControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class SuspendReasonController : GenericController<
        HoshiDbContext, 
        SuspendReason, 
        SuspendReasonGetDTO, 
        SuspendReasonPostDTO, 
        SuspendReasonPutDTO>
    {
        public SuspendReasonController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                SuspendReason, 
                SuspendReasonGetDTO, 
                SuspendReasonPostDTO, 
                SuspendReasonPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
        }
    }
}
