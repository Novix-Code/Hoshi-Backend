using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.Models.OrderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.OrderControllers.InvoiceControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : GenericJustFSPController<
        HoshiDbContext, 
        Invoice, 
        InvoiceGetDTO>
    {
        public InvoiceController(
            IGenericFSPService<
                HoshiDbContext, 
                Invoice, 
                InvoiceGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Invoice.Order)}.{nameof(Order.Client)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.Worker)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.City)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.Service)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(Invoice.Order)}.{nameof(Order.OrderStatusHistory)}",
				$"{nameof(Invoice.OrderVisit)}",
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
