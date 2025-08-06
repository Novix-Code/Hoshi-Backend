using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.AuthService;
using Hoshi.Repositories.Hubs;

using Hoshi.Repositories.UserService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;


        private readonly IUserService userService;
        
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
                UserGetDTO> genericFSPService,
            IAuthService authService
,
            IHubContext<NotificationHub, INotificationHub> hubContext) : base(mapper, genericCRUDService, genericFSPService)
        {
            this.mapper = mapper;
            this.authService = authService;
            _hubContext = hubContext;

            IAuthService authService,
            IUserService userService
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            this.mapper = mapper;
            this.authService = authService;
            this.userService = userService;

        }
        
        public override async Task<IActionResult> Add(UserPostDTO postDTO)
        {
            // Update the other data anyware
            var serviceResponse = await authService.Register(
                postDTO.UserType, 
                mapper.Map<ApplicationUserRegisterRequestDto>(postDTO)
            );
            // Check if image is not null to be updated
            if (postDTO.Image is not null)
            {
                // Update image by adding a the new one
                Tuple<bool, string> result = 
                    await userService.AddUserImage(serviceResponse.Data!.Id, postDTO.Image, false);

                // Chekc if it done successfully or not
                if (result.Item1 is false)
                    return BadRequest(result.Item2);
            }

            return StatusCode(serviceResponse.StatusCode, serviceResponse);
        }

        public override async Task<IActionResult> Update(UserPutDTO putDTO)
        {
            // Check if image is not null to be updated
            if (putDTO.Image is not null)
            {
                // Update image by adding a the new one
                Tuple<bool, string> result = await userService.AddUserImage(putDTO.Id, putDTO.Image, true);

                // Chekc if it done successfully or not
                if (result.Item1 is false)
                    return BadRequest(result.Item2);
            }

            // Update the other data anyware
            return await base.Update(putDTO);
        }

        [HttpGet("OverViewPage")]
        public async Task<IActionResult> overView()
        {
            var reponse = await userService.overViewPage();
            return StatusCode((int)Response.StatusCode, reponse);
        }
        
        [HttpGet("ClientPage")]
        public async Task<IActionResult> clientpage()
        {
            var response = await userService.Clientpage();
            return StatusCode((int)Response.StatusCode, response);  
        }
        
        [HttpGet("ClientDetails{id}")]
        public async Task<IActionResult> clientDetails(int id)
        {
            var response = await userService.ClientDetails(id);    
            return StatusCode((int)response.StatusCode, response);  
        }
        
        [HttpGet("WorkerPage")]
        public async Task<IActionResult> workerPage()
        {
            var response = await userService.WorkerPage();
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("BeWorkerRequest{id}")]
        public async Task<IActionResult> beWorkerReq(int id)
        {
            var response   =  await userService.WorkerDetails(id);
            return StatusCode((int)response.StatusCode, response);   
        }
        
        [HttpPost("BeWorkerApproved{id}")]
        public async Task<IActionResult> beworkerapproved(int id)
        {
            var resonse  = await userService.BeWorkerApproved(id);
            return StatusCode((int)resonse.StatusCode, resonse);
        }
        
        [HttpPost("BeWorkerRejected")]
        public async Task<IActionResult> beworkerreject(int id , string RejectResoun)
        {
            var resonse  = await userService.BeWorkerReject(id , RejectResoun);
            return StatusCode((int)resonse.StatusCode, resonse);
        }
        
        [HttpGet("DashbordWorkerDetails{id}")]
        public async Task<IActionResult> dashWOrker(int id)
        {
            var response = await userService.DashbordWorkerDetails(id);    
            return StatusCode(response.StatusCode, response);   
        }

        [HttpGet("OrdersPage")]
        public async Task<IActionResult> ordrsDetails()
        {
            var response = await userService.OrderPage();
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpGet("DashbordOrderDetails{id}")]
        public async Task<IActionResult> dashorderDetls(int id)
        {
            var response = await userService.OrderDetails(id);
            return StatusCode(response.StatusCode, response);
        }


       
    }
}
