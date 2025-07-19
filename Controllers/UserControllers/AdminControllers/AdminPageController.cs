using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs;
using Hoshi.Models.UserModels.AdminModels;

namespace Hoshi.Controllers.UserControllers.AdminControllers.AdminPageControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class AdminPageController : GenericFSPController<
        HoshiDbContext, 
        AdminPage, 
        AdminPageGetDTO, 
        AdminPagePostDTO, 
        AdminPagePutDTO>
    {
        public AdminPageController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                AdminPage, 
                AdminPageGetDTO, 
                AdminPagePostDTO, 
                AdminPagePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                AdminPage, 
                AdminPageGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(AdminPage.User)}",
				$"{nameof(AdminPage.Page)}.{nameof(Page.ParentPage)}",
			];
        }
    }
}
