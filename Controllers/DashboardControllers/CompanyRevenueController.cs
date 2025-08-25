using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.OrderModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.CompanyRevenueDTOs;
using Hoshi.Models.DashboardModels;

namespace Hoshi.Controllers.DashboardControllers.CompanyRevenueControllers
{
    [NonController]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class CompanyRevenueController : GenericJustFSPController<
        HoshiDbContext, 
        CompanyRevenue, 
        CompanyRevenueGetDTO>
    {
        public CompanyRevenueController(
            IGenericFSPService<
                HoshiDbContext, 
                CompanyRevenue, 
                CompanyRevenueGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.Client)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.Worker)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.City)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.Service)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(CompanyRevenue.Order)}.{nameof(Order.OrderStatusHistory)}",
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
