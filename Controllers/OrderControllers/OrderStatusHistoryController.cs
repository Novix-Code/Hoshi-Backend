using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.Models.OrderModels;

namespace Hoshi.Controllers.OrderControllers.OrderStatusHistoryControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusHistoryController : GenericJustFSPController<
        HoshiDbContext, 
        OrderStatusHistory, 
        OrderStatusHistoryGetDTO>
    {
        public OrderStatusHistoryController(
            IGenericFSPService<
                HoshiDbContext, 
                OrderStatusHistory, 
                OrderStatusHistoryGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.Client)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.Worker)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.City)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.Service)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(OrderStatusHistory.Order)}.{nameof(Order.OrderVisits)}",
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
