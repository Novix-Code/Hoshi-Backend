using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionServiceDTOs;
using Hoshi.Models.PromotionModels;

namespace Hoshi.Controllers.PromotionControllers.PromotionServiceControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class PromotionServiceController : GenericJustFSPController<
        HoshiDbContext, 
        PromotionService, 
        PromotionServiceGetDTO>
    {
        public PromotionServiceController(
            IGenericFSPService<
                HoshiDbContext, 
                PromotionService, 
                PromotionServiceGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(PromotionService.Service)}.{nameof(Service.ServiceCategory)}",
				$"{nameof(PromotionService.Promotion)}",
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
    }
}
