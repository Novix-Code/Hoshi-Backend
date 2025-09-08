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


        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="userType">User type (client/worker).</param>
        /// <param name="registerRequestDto">Registration payload.</param>
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromQuery] UserType userType,
            [FromBody] ApplicationUserRegisterRequestDto registerRequestDto
        )
        {
            var serviceResponse = await authService.Register(userType, registerRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        /// <summary>
        /// Authenticate a user and return a JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginRequestDto loginRequestDto)
        {
            var serviceResponse = await authService.Login(loginRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        /// <summary>
        /// Start password reset by generating and emailing a reset token.
        /// </summary>
        [HttpPost("Change-Password-Request")]
        public async Task<IActionResult> ChangeReqPassword([FromBody] string email)
        {
            var response = await authService.CreateResetPasswordTokenAsync(email);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Reset password using a previously issued token.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
        {
            var response = await authService.ResetPasswordAsync(requestDto);
            await emailService.ReSetOtp(requestDto.Email);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Submit application to become a worker
        /// </summary>
        /// <param name="request">Worker application details</param>
        /// <returns>Result of the application submission</returns>
        [HttpPost("be-worker")]
        //[Authorize]
        public async Task<IActionResult> BeWorker([FromForm] BeWorkerRequestDTO request)
        {
            // Get user ID from JWT token
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int tokenUserId))
            //{
            //    return Unauthorized(new
            //    {
            //        ErrorAr = "غير مصرح بالوصول.",
            //        ErrorEn = "Unauthorized access."
            //    });
            //}

            //// Ensure the user can only apply for themselves
            //if (request.UserId != tokenUserId)
            //    return Forbid();
            

            var result = await authService.BeWorkerAsync(request);
            return StatusCode(result.StatusCode, result);
        }
        
        

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var serviceResponse = await authService.Logout();
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        /// <summary>
        /// Get the authenticated user's id from JWT.
        /// </summary>
        [HttpGet("get-current-userId")]
        public IActionResult GetUserId()
        {
            var response = authService.GetCurrentUserId();
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Edit profile data for the authenticated user.
        /// </summary>
        [HttpPatch("edit")]
     
        public async Task<IActionResult> Edit([FromForm] ApplicationUserEditRequestDto userEditRequestDto)
        {
            var serviceResponse = await authService.Edit(userEditRequestDto);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }

        /// <summary>
        /// Delete a user (admin only).
        /// </summary>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var serviceResponse = await authService.Delete(id);
            return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
        }
        /// <summary>
        /// Verify OTP code received via email.
        /// </summary>
        [HttpPost("CheckOTP")]
        public async Task<IActionResult> otpResult(string otp, string id)
        {
            var response = await emailService.checkOTPVerfication(otp, id);
            return StatusCode(response.StatusCode, response);
        }
        /// <summary>
        /// Request to resend OTP.
        /// </summary>
        [HttpPost("reset-OTP")]
        public async Task<IActionResult> resetOtp(string Email)
        {
            var repsonse = await emailService.ReSetOtp(Email);
            return StatusCode(repsonse.StatusCode, Response);
        }

        /// <summary>
        /// Get admins along with their roles and permissions.
        /// </summary>
        [HttpGet("get-all-admins-with-roles-and-permissions")]
        public async Task<IActionResult> GetAllAdminsWithRolesAndPermissionsAsync()
        {
            var response = await userService.GetAllAdminsWithRolesAndPermissionsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
