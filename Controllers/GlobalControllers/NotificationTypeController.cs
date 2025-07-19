using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Controllers.GlobalControllers.NotificationTypeControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class NotificationTypeController : GenericFSPController<
        HoshiDbContext, 
        NotificationType, 
        NotificationTypeGetDTO, 
        NotificationTypePostDTO, 
        NotificationTypePutDTO>
    {
        public NotificationTypeController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                NotificationType, 
                NotificationTypeGetDTO, 
                NotificationTypePostDTO, 
                NotificationTypePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                NotificationType, 
                NotificationTypeGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
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
    }
}
