using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.NumericalStatisticsValueDTOs;
using Hoshi.Models.DashboardMdoels.StatisticsModels;

namespace Hoshi.Controllers.DashboardMdoels.StatisticsControllers.NumericalStatisticsValueControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class NumericalStatisticsValueController : GenericJustFSPController<
        HoshiDbContext, 
        NumericalStatisticsValue, 
        NumericalStatisticsValueGetDTO>
    {
        public NumericalStatisticsValueController(
            IGenericFSPService<
                HoshiDbContext, 
                NumericalStatisticsValue, 
                NumericalStatisticsValueGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(NumericalStatisticsValue.LastValue)}",
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
