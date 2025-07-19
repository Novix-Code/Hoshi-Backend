using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionPageDTOs;
using Hoshi.Models.UserModels.AdminModels;

namespace Hoshi.Controllers.UserControllers.AdminControllers.PermissionPageControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class PermissionPageController : GenericController<
        HoshiDbContext, 
        PermissionPage, 
        PermissionPageGetDTO, 
        PermissionPagePostDTO, 
        PermissionPagePutDTO>
    {
        public PermissionPageController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                PermissionPage, 
                PermissionPageGetDTO, 
                PermissionPagePostDTO, 
                PermissionPagePutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(PermissionPage.Page)}.{nameof(Page.ParentPage)}",
				$"{nameof(PermissionPage.Permission)}",
			];
        }
    }
}
