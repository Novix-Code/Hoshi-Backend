using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.OrderComplaetionRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.OrderComplaetionRateControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class OrderComplaetionRateController : GenericJustFSPController<
        HoshiDbContext, 
        OrderComplaetionRate, 
        OrderComplaetionRateGetDTO>
    {
        public OrderComplaetionRateController(
            IGenericFSPService<
                HoshiDbContext, 
                OrderComplaetionRate, 
                OrderComplaetionRateGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(OrderComplaetionRate.ServiceCategory)}",
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
