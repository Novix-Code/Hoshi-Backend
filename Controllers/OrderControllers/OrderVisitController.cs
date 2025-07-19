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
        public override Task<IActionResult> Add(OrderVisitPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Worker")]
        public override Task<IActionResult> Update(OrderVisitPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
