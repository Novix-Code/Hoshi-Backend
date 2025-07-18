using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.Repositories.OrderService;

using Hoshi.Repositories.WorkerOrderService;

using Hoshi.Repositories.ClientOrderService;

using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.Models.OrderModels;

namespace Hoshi.Controllers.OrderControllers.OrderControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : GenericFSPController<
        HoshiDbContext, 
        Order, 
        OrderGetDTO, 
        OrderPostDTO, 
        OrderPutDTO>
    {
		private readonly IClientOrderService clientOrderService;
		private readonly IWorkerOrderService workerOrderService;
		private readonly IOrderService orderService;
        public OrderController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Order, 
                OrderGetDTO, 
                OrderPostDTO, 
                OrderPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Order, 
                OrderGetDTO> genericFSPService,
			IClientOrderService clientOrderService,
			IWorkerOrderService workerOrderService,
			IOrderService orderService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Order.Client)}",
				$"{nameof(Order.Worker)}",
				$"{nameof(Order.City)}",
				$"{nameof(Order.Service)}.{nameof(Service.ServiceCategory)}",
				$"{nameof(Order.AppliedPromotion)}",
				$"{nameof(Order.OrderImages)}",
				$"{nameof(Order.OrderVisits)}",
				$"{nameof(Order.OrderStatusHistory)}",
			];
			this.clientOrderService = clientOrderService;
			this.workerOrderService = workerOrderService;
			this.orderService = orderService;
        }

        [EndpointGroupName("Client")]
        public override async Task<IActionResult> Delete(int id)
        {
            var response  = await clientOrderService.DeleteOrder(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<OrderPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Client")]
        public override async Task<IActionResult> Add(OrderPostDTO postDTO)
        {
            var response = await clientOrderService.AddOrderAsync(postDTO);
            return StatusCode((int)response.StatusCode, response);
            
        }
        [EndpointGroupName("Client")]
        public override async Task<IActionResult> GetAll()
        {
            var response = await clientOrderService.GetAllClientsAsync();
            return StatusCode((int)response.StatusCode,response);   
        }

        [EndpointGroupName("Client")]
        public override async Task<IActionResult> GetById(int id)
        {
            var response = await clientOrderService.GetOrderDetails(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [EndpointGroupName("Client")]
        public override Task<IActionResult> Update(OrderPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
