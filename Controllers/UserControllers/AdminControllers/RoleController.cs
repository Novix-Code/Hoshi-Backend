using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs;
using Hoshi.Models.UserModels.AdminModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.AdminControllers.RoleControllers
{
    [Authorize]
    [NonController]
    //[ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class RoleController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        Role, 
        RoleGetDTO, 
        RolePostDTO, 
        RolePutDTO>
    {
        public RoleController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Role, 
                RoleGetDTO, 
                RolePostDTO, 
                RolePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Role, 
                RoleGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
        }
    }
}
