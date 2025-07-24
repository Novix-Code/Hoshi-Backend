using System.Security.Claims;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Repositories.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
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

    }
}
