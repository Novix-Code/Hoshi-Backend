using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs;
using Hoshi.Models.GlobalModels;
using Hoshi.Repositories.Hubs;
using Microsoft.AspNetCore.SignalR;
using Hoshi.Repositories.NotificationService;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.GlobalControllers.UserNotificationControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserNotificationController : GenericFSPController<
        HoshiDbContext, 
        UserNotification, 
        UserNotificationGetDTO, 
        UserNotificationPostDTO, 
        UserNotificationPutDTO>
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler _notificationServiceHandler;
        public UserNotificationController(
            IMapper mapper,
            IGenericCRUDService<
                HoshiDbContext,
                UserNotification,
                UserNotificationGetDTO,
                UserNotificationPostDTO,
                UserNotificationPutDTO> genericCRUDService,
            IGenericFSPService<
                HoshiDbContext,
                UserNotification,
                UserNotificationGetDTO> genericFSPService
,
            IHubContext<NotificationHub, INotificationHub> hubContext,
            INotificationServiceHandler notificationServiceHandler) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

            includes = [
                $"{nameof(UserNotification.User)}",
                $"{nameof(UserNotification.NotificationType)}",
            ];
            _hubContext = hubContext;
            _notificationServiceHandler = notificationServiceHandler;
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

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(UserNotificationPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<UserNotificationPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(UserNotificationPutDTO putDTO)
        {
            return base.Update(putDTO);
        }


        [Authorize(Roles = "admin")]
        [EndpointGroupName("Admin")]
        [HttpGet("Get-Admin-Notifications")]
        public async Task<IActionResult> adminNots()
        {
            var response = await _notificationServiceHandler.getNotificationsAdminAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        [Authorize(Roles = "worker,client")]

        [EndpointGroupName("Admin")]
        [HttpGet("Get-WorkerAndClient-Notifications")]
        public async Task<IActionResult> WorkersClients()
        {
            var response = await _notificationServiceHandler.getNotificationsClientAndWorkerAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        //[HttpPost("SendMS")]
        //public async Task<IActionResult> SendMessage([FromForm] string message)
        //{
        //    await _hubContext.Clients.All.ReceiveMessage(message);
        //    return Ok(new { Message = "Sent" });
        //}
    }
}
