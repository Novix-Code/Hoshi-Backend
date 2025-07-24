using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.Repositories.WorkerHomeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;

namespace Hoshi.Controllers.UserControllers.WorkerControllers
{
    [Route("api/[controller]")]
    [EndpointGroupName("Worker")]
    [ApiController]
    public class WorkerHomeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkerHomeService _workerHomeService;
        public WorkerHomeController(IWorkerHomeService workerHomeService, IHttpContextAccessor httpContextAccessor)
        {
            _workerHomeService = workerHomeService;
            _httpContextAccessor = httpContextAccessor;

        }

        [Authorize]
        [HttpGet("get-worker-home")]
        public async Task<IActionResult> GetWorkerHome()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int workerId))
                return Unauthorized(ResultDTO<string>.Unauthorized());

            var result = await _workerHomeService.GetWorkerHomeAsync(workerId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("search-orders")]
        public async Task<IActionResult> SearchOrders([FromBody] OrderSearchRequestDto searchRequest)
        {
            var result = await _workerHomeService.SearchOrdersAsync(searchRequest);
            return StatusCode(result.StatusCode, result);
        }
    }
}
