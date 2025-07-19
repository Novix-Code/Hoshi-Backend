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
using Microsoft.AspNetCore.Authorization;

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

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<OrderPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Client")]
        public override Task<IActionResult> Add(OrderPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Client")]
        public override Task<IActionResult> Update(OrderPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
        
        [Authorize]
        [HttpGet("order-details")]
        public async Task<IActionResult> GetSubmittedOrderDetails([FromQuery] int orderId)
        {
	        var result = await orderService.GetSubmittedOrderDetailsAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("order-client-details")]
        public async Task<IActionResult> GetOrderClientDetails([FromQuery] int orderId)
        {
	        var result = await orderService.GetOrderClientDetailsAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("complete-order")]
        public async Task<IActionResult> CompleteOrder([FromQuery] int orderId)
        {
	        var result = await orderService.CompleteOrderAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet("assigned-order")]
        public async Task<IActionResult> GetAssignedOrder([FromQuery] int orderId)
        {
	        var result = await orderService.GetAssignedOrderAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
    }
}
