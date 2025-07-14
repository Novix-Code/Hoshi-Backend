using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;
using Hoshi.Repositories.WorkerWalletService;

using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerWalletControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerWalletController : GenericFSPController<
        HoshiDbContext, 
        WorkerWallet, 
        WorkerWalletGetDTO, 
        WorkerWalletPostDTO, 
        WorkerWalletPutDTO>
    {
		private readonly IWorkerWalletService workerWalletService;
        public WorkerWalletController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerWallet, 
                WorkerWalletGetDTO, 
                WorkerWalletPostDTO, 
                WorkerWalletPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                WorkerWallet, 
                WorkerWalletGetDTO> genericFSPService,
			IWorkerWalletService workerWalletService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerWallet.Worker)}",
			];
			this.workerWalletService = workerWalletService;
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
