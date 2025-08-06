using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.Repositories.OrderVisitService;

using Hoshi.Repositories.WorkerVisitService;

using Hoshi.Repositories.ClientVisitService;

using Hoshi.Models.OrderModels;

namespace Hoshi.Controllers.OrderControllers.OrderVisitControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderVisitController : GenericFSPController<
        HoshiDbContext, 
        OrderVisit, 
        OrderVisitGetDTO, 
        OrderVisitPostDTO, 
        OrderVisitPutDTO>
    {
		private readonly IClientVisitService clientVisitService;
		private readonly IWorkerVisitService workerVisitService;
		private readonly IOrderVisitService orderVisitService;
        public OrderVisitController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                OrderVisit, 
                OrderVisitGetDTO, 
                OrderVisitPostDTO, 
                OrderVisitPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                OrderVisit, 
                OrderVisitGetDTO> genericFSPService,
			IClientVisitService clientVisitService,
			IWorkerVisitService workerVisitService,
			IOrderVisitService orderVisitService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
			this.clientVisitService = clientVisitService;
			this.workerVisitService = workerVisitService;
			this.orderVisitService = orderVisitService;
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

        [EndpointGroupName("Worker")]
        public override async Task<IActionResult> Add(OrderVisitPostDTO postDTO)
        {
            await orderVisitService.sendNoificationforclient(postDTO.OrderId , "تم ارسال طلب زيارة");
            
            return await base.Add(postDTO);
        }

        [EndpointGroupName("Worker")]
        public override Task<IActionResult> Update(OrderVisitPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
        
        [HttpPost("add-visit")]
        public async Task<IActionResult> AddVisit([FromBody] OrderVisitPostDTO dto)
        {
            var result = await workerVisitService.AddVisitAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("complete-visit")]
        public async Task<IActionResult> CompleteVisit([FromQuery] int visitId)
        {
            await orderVisitService.sendNoificationforclient2(visitId , "تم استكمال طلب الزيارة");
            var result = await workerVisitService.CompleteVisitAsync(visitId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("cancel-visit")]
        public async Task<IActionResult> CancelVisit([FromQuery] int visitId)
        {
            var result = await workerVisitService.CancelVisitAsync(visitId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
