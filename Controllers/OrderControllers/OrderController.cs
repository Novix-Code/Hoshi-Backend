using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Models.ServiceModels;
using Hoshi.Repositories.ClientOrderService;
using Hoshi.Repositories.OrderService;
using Hoshi.Repositories.WorkerOrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.OrderControllers.OrderControllers
{

    /// <summary>
    /// Order endpoints built on top of generic CRUD and FSP services.
    /// Client endpoints delegate to domain services to encapsulate business logic and constraints.
    /// </summary>
    [Authorize]
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
        /// <summary>
        /// Delete an order (client scope).
        /// </summary>
        public override async Task<IActionResult> Delete(int id)
        {
            var response  = await clientOrderService.DeleteOrder(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [EndpointGroupName("Client")]
        /// <summary>
        /// Create a new order (client scope).
        /// </summary>
        public override async Task<IActionResult> Add([FromForm] OrderPostDTO postDTO)
        {

            var response = await clientOrderService.AddOrderAsync(postDTO);
            return StatusCode((int)response.StatusCode, response);
            
        }

        [EndpointGroupName("Client")]
        /// <summary>
        /// Get all client orders.
        /// </summary>
        public override async Task<IActionResult> GetAll()
        {
            var response = await clientOrderService.GetAllClientsAsync();
            return StatusCode((int)response.StatusCode,response);   
        }

        [EndpointGroupName("Client")]
        /// <summary>
        /// Get order by id (client scope).
        /// </summary>
        public override async Task<IActionResult> GetById(int id)
        {
            var response = await clientOrderService.GetOrderDetails(id);
            return StatusCode((int)response.StatusCode, response);
        }
        
        //[Authorize]
        [HttpGet("order-details")]
        /// <summary>
        /// Get details of a submitted order (client-facing view model).
        /// </summary>
        public async Task<IActionResult> GetSubmittedOrderDetails([FromQuery] int orderId)
        {
	        var result = await orderService.GetSubmittedOrderDetailsAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        //[Authorize]
        [HttpGet("order-client-details")]
        /// <summary>
        /// Get public profile details for the client who submitted an order.
        /// </summary>
        public async Task<IActionResult> GetOrderClientDetails([FromQuery] int orderId)
        {
	        var result = await orderService.GetOrderClientDetailsAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        //[Authorize]
        [HttpPost("complete-order")]
        /// <summary>
        /// Mark an order as completed, handling financial operations and notifications.
        /// </summary>
        public async Task<IActionResult> CompleteOrder([FromQuery] int orderId)
        {
	        var result = await orderService.CompleteOrderAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet("assigned-order")]
        /// <summary>
        /// Get the currently assigned order details.
        /// </summary>
        public async Task<IActionResult> GetAssignedOrder([FromQuery] int orderId)
        {
	        var result = await orderService.GetAssignedOrderAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }
        
        [HttpGet("get-dashboard-order-details")]
        /// <summary>
        /// Get rich order details for admin dashboard.
        /// </summary>
        public async Task<IActionResult> GetDashboardOrderDetails([FromQuery] int orderId)
        {
	        var result = await orderService.GetDashboardOrderDetailsAsync(orderId);
	        return StatusCode(result.StatusCode, result);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<OrderPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override Task<IActionResult> Update(OrderPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
