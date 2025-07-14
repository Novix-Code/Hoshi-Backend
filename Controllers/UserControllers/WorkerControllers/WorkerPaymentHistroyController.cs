using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;
using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerPaymentHistroyControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerPaymentHistroyController : GenericFSPController<
        HoshiDbContext, 
        WorkerPaymentHistroy, 
        WorkerPaymentHistroyGetDTO, 
        WorkerPaymentHistroyPostDTO, 
        WorkerPaymentHistroyPutDTO>
    {
        public WorkerPaymentHistroyController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerPaymentHistroy, 
                WorkerPaymentHistroyGetDTO, 
                WorkerPaymentHistroyPostDTO, 
                WorkerPaymentHistroyPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                WorkerPaymentHistroy, 
                WorkerPaymentHistroyGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerPaymentHistroy.Worker)}",
			];
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
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
