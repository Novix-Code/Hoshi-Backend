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
using Hoshi.Repositories.EmailServiceFold;
using Microsoft.AspNetCore.Authorization;
using Hoshi.DTOs.UserDTOs.UserRegistiration;

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
		private readonly IUserService _userService;
        private readonly IEmailService _emailService;

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
			this._userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] ApplicationUserRegisterRequestDto registerRequestDto)
        {
            var serviceResponse = await _userService.Register(registerRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginRequestDto loginRequestDto)
        {
            var serviceResponse = await _userService.Login(loginRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var serviceResponse = await _userService.Delete(id);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPatch("edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] ApplicationUserEditRequestDto userEditRequestDto)
        {
            var serviceResponse = await _userService.Edit(userEditRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var serviceResponse = await _userService.GetById(id);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var response = await _userService.CreateResetPasswordTokenAsync(email);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
        {
            var response = await _userService.ResetPasswordAsync(requestDto);
            await _emailService.SendVerifivationCode(requestDto.Email);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("get-current-userId")]
        public IActionResult GetUserId()
        {
            var response = _userService.GetCurrentUserId();
            return StatusCode((int)response.StatusCode, response);
        }


        //[HttpPost("ban/{userId}")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> BanUser([FromRoute] string userId)
        //{
        //    var serviceResponse = await _userService.BanUserAsync(userId);
        //    return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        //}

        //[HttpPost("unban/{userId}")]
        //[Authorize(Roles = "admin")]
        //public async Task<IActionResult> UnbanUser([FromRoute] string userId)
        //{
        //    var serviceResponse = await _userService.UnbanUserAsync(userId);
        //    return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        //}

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var serviceResponse = await _userService.Logout();
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }


    }
}
