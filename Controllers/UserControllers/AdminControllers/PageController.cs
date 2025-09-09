using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs;
using Hoshi.Models.UserModels.AdminModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.AdminControllers.PageControllers
{
    [Authorize]
    [NonController]
    //[ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class PageController : SoftDeleteGenericController<
        HoshiDbContext, 
        Page, 
        PageGetDTO, 
        PagePostDTO, 
        PagePutDTO>
    {
        public PageController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Page, 
                PageGetDTO, 
                PagePostDTO, 
                PagePutDTO> genericCRUDService
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(Page.ParentPage)}",
			];
        }
    }
}
