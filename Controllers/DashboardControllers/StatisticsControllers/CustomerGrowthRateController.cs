using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CustomerGrowthRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.CustomerGrowthRateControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class CustomerGrowthRateController : GenericJustFSPController<
        HoshiDbContext, 
        CustomerGrowthRate, 
        CustomerGrowthRateGetDTO>
    {
        public CustomerGrowthRateController(
            IGenericFSPService<
                HoshiDbContext, 
                CustomerGrowthRate, 
                CustomerGrowthRateGetDTO> genericFSPService 
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
