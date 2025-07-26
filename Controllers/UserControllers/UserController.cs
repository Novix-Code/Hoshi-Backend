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


    }
}
