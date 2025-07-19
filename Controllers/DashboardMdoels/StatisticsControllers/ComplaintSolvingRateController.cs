using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.ComplaintSolvingRateDTOs;
using Hoshi.Models.DashboardMdoels.StatisticsModels;

namespace Hoshi.Controllers.DashboardMdoels.StatisticsControllers.ComplaintSolvingRateControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ComplaintSolvingRateController : GenericJustFSPController<
        HoshiDbContext, 
        ComplaintSolvingRate, 
        ComplaintSolvingRateGetDTO>
    {
        public ComplaintSolvingRateController(
            IGenericFSPService<
                HoshiDbContext, 
                ComplaintSolvingRate, 
                ComplaintSolvingRateGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(ComplaintSolvingRate.ComplaintType)}",
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
