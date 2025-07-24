using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsValueDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.NumericalStatisticsValueControllers
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
