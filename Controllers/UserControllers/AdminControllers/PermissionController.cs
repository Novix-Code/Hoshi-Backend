using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.Models.UserModels.AdminModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.AdminControllers.PermissionControllers
{
    [Authorize]
    [NonController]
    //[ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class PermissionController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        Permission, 
        PermissionGetDTO, 
        PermissionPostDTO, 
        PermissionPutDTO>
    {
        public PermissionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Permission, 
                PermissionGetDTO, 
                PermissionPostDTO, 
                PermissionPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Permission, 
                PermissionGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
        }
    }
}
