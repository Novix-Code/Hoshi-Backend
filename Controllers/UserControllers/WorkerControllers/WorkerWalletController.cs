using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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

        /// <summary>
        /// Get Worker Wallet - Returns balance and wallet history
        /// </summary>
        [Authorize]
        [HttpGet("get-worker-wallet")]
        public async Task<IActionResult> GetWorkerWallet()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int workerId))
                return Unauthorized();

            var result = await workerWalletService.GetWorkerWalletAsync(workerId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Add Payment - Submit bill image for payment request
        /// </summary>
        [Authorize]
        [HttpPost("add-payment")]
        public async Task<IActionResult> AddPayment([FromForm] AddPaymentRequestDTO request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int workerId))
                return Unauthorized();

            var result = await workerWalletService.AddPaymentAsync(workerId, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
