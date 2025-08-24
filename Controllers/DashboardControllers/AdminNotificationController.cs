using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.AdminNotificationDTOs;
using Hoshi.Models.DashboardModels;

namespace Hoshi.Controllers.DashboardControllers.AdminNotificationControllers
{
    [NonController]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class AdminNotificationController : GenericController<
        HoshiDbContext, 
        AdminNotification, 
        AdminNotificationGetDTO, 
        AdminNotificationPostDTO, 
        AdminNotificationPutDTO>
    {
        public AdminNotificationController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                AdminNotification, 
                AdminNotificationGetDTO, 
                AdminNotificationPostDTO, 
                AdminNotificationPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(AdminNotification.Admin)}",
			];
        }
    }
}
