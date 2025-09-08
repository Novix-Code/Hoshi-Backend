using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.IncomeGrowthRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.IncomeGrowthRateControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class IncomeGrowthRateController : GenericJustFSPController<
        HoshiDbContext, 
        IncomeGrowthRate, 
        IncomeGrowthRateGetDTO>
    {
        public IncomeGrowthRateController(
            IGenericFSPService<
                HoshiDbContext, 
                IncomeGrowthRate, 
                IncomeGrowthRateGetDTO> genericFSPService 
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
