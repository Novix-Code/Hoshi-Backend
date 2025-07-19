using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.NumericalStatisticsDTOs;
using Hoshi.Models.DashboardMdoels.StatisticsModels;

namespace Hoshi.Controllers.DashboardMdoels.StatisticsControllers.NumericalStatisticsControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class NumericalStatisticsController : GenericJustFSPController<
        HoshiDbContext, 
        NumericalStatistics, 
        NumericalStatisticsGetDTO>
    {
        public NumericalStatisticsController(
            IGenericFSPService<
                HoshiDbContext, 
                NumericalStatistics, 
                NumericalStatisticsGetDTO> genericFSPService 
        ) : base(genericFSPService)
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
