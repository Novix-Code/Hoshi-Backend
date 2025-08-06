using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Enums;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.AuthService;
using Hoshi.Repositories.Hubs;
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
        }

        public override async Task<IActionResult> Add(UserPostDTO postDTO)
        {
            var serviceResponse = await authService.Register(
                postDTO.UserType, 
                mapper.Map<ApplicationUserRegisterRequestDto>(postDTO)
            );

            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }


       
    }
}
