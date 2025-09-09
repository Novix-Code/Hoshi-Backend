using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.Models.UserModels.AdminModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.AdminControllers.UserPermissionControllers
{
    [Authorize]
    [NonController]
    //[ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class UserPermissionController : GenericController<
        HoshiDbContext, 
        UserPermission, 
        UserPermissionGetDTO, 
        UserPermissionPostDTO, 
        UserPermissionPutDTO>
    {
        public UserPermissionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                UserPermission, 
                UserPermissionGetDTO, 
                UserPermissionPostDTO, 
                UserPermissionPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(UserPermission.User)}",
				$"{nameof(UserPermission.Permission)}",
			];
        }
    }
}
