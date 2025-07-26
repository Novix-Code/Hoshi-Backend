using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers.UserControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class UserController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        User, 
        UserGetDTO, 
        UserPostDTO, 
        UserPutDTO>
    {
        private readonly IUserService _userService;
        public UserController(
            IMapper mapper,
            IGenericCRUDService<
                HoshiDbContext,
                User,
                UserGetDTO,
                UserPostDTO,
                UserPutDTO> genericCRUDService,
            IGenericFSPService<
                HoshiDbContext,
                User,
                UserGetDTO> genericFSPService
,
            IUserService userService) : base(mapper, genericCRUDService, genericFSPService)
        {
            _userService = userService;
        }


        [HttpGet("OverViewPage")]
        public async Task<IActionResult> overView()
        {
            var reponse = await _userService.overViewPage();
            return StatusCode((int)Response.StatusCode, reponse);
        }
        [HttpGet("ClientPage")]
        public async Task<IActionResult> clientpage()
        {
            var response = await _userService.Clientpage();
            return StatusCode((int)Response.StatusCode, response);  
        }
        [HttpGet("ClientDetails{id}")]
        public async Task<IActionResult> clientDetails(int id)
        {
            var response = await _userService.ClientDetails(id);    
            return StatusCode((int)response.StatusCode, response);  
        }
        [HttpGet("WorkerPage")]
        public async Task<IActionResult> workerPage()
        {
            var response = await _userService.WorkerPage();
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("BeWorkerRequest{id}")]
        public async Task<IActionResult> beWorkerReq(int id)
        {
            var response   =  await _userService.WorkerDetails(id);
            return StatusCode((int)response.StatusCode, response);   
        }
        [HttpPost("BeWorkerApproved{id}")]
        public async Task<IActionResult> beworkerapproved(int id)
        {
            var resonse  = await _userService.BeWorkerApproved(id);
            return StatusCode((int)resonse.StatusCode, resonse);
        }
        [HttpPost("BeWorkerRejected")]
        public async Task<IActionResult> beworkerreject(int id , string RejectResoun)
        {
            var resonse  = await _userService.BeWorkerReject(id , RejectResoun);
            return StatusCode((int)resonse.StatusCode, resonse);
        }
        [HttpGet("DashbordWorkerDetails{id}")]
        public async Task<IActionResult> dashWOrker(int id)
        {
            var response = await _userService.DashbordWorkerDetails(id);    
            return StatusCode(response.StatusCode, response);   
        }

        [HttpGet("OrdersPage")]
        public async Task<IActionResult> ordrsDetails()
        {
            var response = await _userService.OrderPage();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("DashbordOrderDetails{id}")]
        public async Task<IActionResult> dashorderDetls(int id)
        {
            var response = await _userService.OrderDetails(id);
            return StatusCode(response.StatusCode, response);
        }



    }
}
