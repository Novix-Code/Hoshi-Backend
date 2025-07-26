using System.Security.Claims;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Repositories.AuthService;
using Hoshi.Repositories.EmailServiceFold;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            IEmailService emailService
        )
        {
            this.authService = authService;
            this.userService = userService;
            this.emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromQuery] UserType userType,
            [FromBody] ApplicationUserRegisterRequestDto registerRequestDto
        )
        {
            var serviceResponse = await authService.Register(userType, registerRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginRequestDto loginRequestDto)
        {
            var serviceResponse = await authService.Login(loginRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var response = await authService.CreateResetPasswordTokenAsync(email);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
        {
            var response = await authService.ResetPasswordAsync(requestDto);
            await emailService.SendVerifivationCode(requestDto.Email);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Submit application to become a worker
        /// </summary>
        /// <param name="request">Worker application details</param>
        /// <returns>Result of the application submission</returns>
        [HttpPost("be-worker")]
        [Authorize]
        public async Task<IActionResult> BeWorker([FromBody] BeWorkerRequestDTO request)
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int tokenUserId))
            {
                return Unauthorized(new
                {
                    ErrorAr = "غير مصرح بالوصول.",
                    ErrorEn = "Unauthorized access."
                });
            }

            // Ensure the user can only apply for themselves
            if (request.UserId != tokenUserId)
                return Forbid();
            

            var result = await authService.BeWorkerAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }
        
        

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var serviceResponse = await authService.Logout();
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet("get-user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var serviceResponse = await authService.GetById(id);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet("get-current-userId")]
        public IActionResult GetUserId()
        {
            var response = authService.GetCurrentUserId();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] ApplicationUserEditRequestDto userEditRequestDto)
        {
            var serviceResponse = await authService.Edit(userEditRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var serviceResponse = await authService.Delete(id);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        [HttpGet("get-all-admins-with-roles-and-permissions")]
        public async Task<IActionResult> GetAllAdminsWithRolesAndPermissionsAsync()
        {
            var response = await userService.GetAllAdminsWithRolesAndPermissionsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
