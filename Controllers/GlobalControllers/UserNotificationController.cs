using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Controllers.GlobalControllers.UserNotificationControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserNotificationController : GenericFSPController<
        HoshiDbContext, 
        UserNotification, 
        UserNotificationGetDTO, 
        UserNotificationPostDTO, 
        UserNotificationPutDTO>
    {
        public UserNotificationController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                UserNotification, 
                UserNotificationGetDTO, 
                UserNotificationPostDTO, 
                UserNotificationPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                UserNotification, 
                UserNotificationGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(UserNotification.User)}",
				$"{nameof(UserNotification.NotificationType)}",
			];
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
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
        public override Task<IActionResult> Add(UserNotificationPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<UserNotificationPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(UserNotificationPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
