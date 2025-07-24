using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CategoryRequestRateDTOs;
using Hoshi.Models.DashboardModels.StatisticsModels;

namespace Hoshi.Controllers.DashboardControllers.StatisticsControllers.CategoryRequestRateControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class CategoryRequestRateController : GenericJustFSPController<
        HoshiDbContext, 
        CategoryRequestRate, 
        CategoryRequestRateGetDTO>
    {
        public CategoryRequestRateController(
            IGenericFSPService<
                HoshiDbContext, 
                CategoryRequestRate, 
                CategoryRequestRateGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(CategoryRequestRate.ServiceCategory)}",
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
