using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.Repositories.UserService;

using Hoshi.Repositories.AuthService;

using Hoshi.Models.UserModels;

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
		private readonly IAuthService authService;
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
			IAuthService authService,
			IUserService userService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
			this.authService = authService;
			this.userService = userService;
        }
    }
}
