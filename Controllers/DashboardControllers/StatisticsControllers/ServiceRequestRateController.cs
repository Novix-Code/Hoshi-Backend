using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ServiceRequestRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.ServiceRequestRateControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class ServiceRequestRateController : GenericJustFSPController<
        HoshiDbContext, 
        ServiceRequestRate, 
        ServiceRequestRateGetDTO>
    {
        public ServiceRequestRateController(
            IGenericFSPService<
                HoshiDbContext, 
                ServiceRequestRate, 
                ServiceRequestRateGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(ServiceRequestRate.Service)}.{nameof(Service.ServiceCategory)}",
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
