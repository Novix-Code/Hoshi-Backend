using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.AuthService;
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
        private readonly IMapper mapper;
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
    }
}
