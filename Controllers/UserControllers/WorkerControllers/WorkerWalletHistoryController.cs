using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerWalletHistoryControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerWalletHistoryController : GenericJustFSPController<
        HoshiDbContext, 
        WorkerWalletHistory, 
        WorkerWalletHistoryGetDTO>
    {
        public WorkerWalletHistoryController(
            IGenericFSPService<
                HoshiDbContext, 
                WorkerWalletHistory, 
                WorkerWalletHistoryGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerWalletHistory.WorkerWallet)}",
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
