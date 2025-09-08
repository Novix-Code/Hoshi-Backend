using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.WorkerPaymentHistroyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerPaymentHistroyControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerPaymentHistroyController : GenericFSPController<
        HoshiDbContext, 
        WorkerPaymentHistroy, 
        WorkerPaymentHistroyGetDTO, 
        WorkerPaymentHistroyPostDTO, 
        WorkerPaymentHistroyPutDTO>
    {
        private readonly IWorkerPaymentHistroyService paymentHistroyService;

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
                WorkerPaymentHistroyGetDTO> genericFSPService,
            IWorkerPaymentHistroyService paymentHistroyService
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerPaymentHistroy.Worker)}",
			];
            this.paymentHistroyService = paymentHistroyService;
        }

        [EndpointGroupName("Worker")]
        public override async Task<IActionResult> Add(WorkerPaymentHistroyPostDTO postDTO)
        {
            var result = await paymentHistroyService.AddService(postDTO);
            return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Worker")]
        public override async Task<IActionResult> Update(WorkerPaymentHistroyPutDTO putDTO)
        {
            var result = await paymentHistroyService.UpdateService(putDTO);
            return StatusCode(result.StatusCode, result);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<WorkerPaymentHistroyPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
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
