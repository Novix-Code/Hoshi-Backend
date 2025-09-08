using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserCollectionAlertDTOs;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.UserCollectionAlertControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserCollectionAlertController : GenericFSPController<
        HoshiDbContext, 
        UserCollectionAlert, 
        UserCollectionAlertGetDTO, 
        UserCollectionAlertPostDTO, 
        UserCollectionAlertPutDTO>
    {
        public UserCollectionAlertController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                UserCollectionAlert, 
                UserCollectionAlertGetDTO, 
                UserCollectionAlertPostDTO, 
                UserCollectionAlertPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                UserCollectionAlert, 
                UserCollectionAlertGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(UserCollectionAlert.User)}",
			];
        }

        [NonAction]
        public override IActionResult Pagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] bool ascending = true)
        {
            return base.Pagination(pageNumber, pageSize, ascending);
        }

        [NonAction]
        public override IActionResult PaginationFilteredSearch(PaginationFilteredSearchDTO paginationFilteredSearchDTO)
        {
            return base.PaginationFilteredSearch(paginationFilteredSearchDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(UserCollectionAlertPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<UserCollectionAlertPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(UserCollectionAlertPutDTO putDTO)
        {
            return base.Update(putDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
