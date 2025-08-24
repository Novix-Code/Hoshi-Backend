using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.RolePermissionDTOs;
using Hoshi.Models.UserModels.AdminModels;

namespace Hoshi.Controllers.UserControllers.AdminControllers.RolePermissionControllers
{
    [NonController]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class RolePermissionController : GenericController<
        HoshiDbContext, 
        RolePermission, 
        RolePermissionGetDTO, 
        RolePermissionPostDTO, 
        RolePermissionPutDTO>
    {
        public RolePermissionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                RolePermission, 
                RolePermissionGetDTO, 
                RolePermissionPostDTO, 
                RolePermissionPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(RolePermission.Role)}",
				$"{nameof(RolePermission.Permission)}",
			];
        }
    }
}
