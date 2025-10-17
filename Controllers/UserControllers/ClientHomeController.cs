using AutoMapper;
using Hoshi.Repositories.ClientHomeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [EndpointGroupName("Client")]
    public class ClientHomeController : ControllerBase
    {
        private readonly IClientHomeService clientHomeService;

        public ClientHomeController(IClientHomeService clientHomeService)
        {
            this.clientHomeService = clientHomeService;
        }

        [HttpGet("GetClientHome")]
        public async Task<IActionResult> GetClientHome(int clientId)
        {
            var response = await clientHomeService.ClientHomePage(clientId);
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("GetGuestHomeService")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClientHomeByClientId()
        {
            var response = await clientHomeService.ClientHomePageForGuest();
            return StatusCode((int)response.StatusCode, response);
        }
        
        // [HttpGet("GetAllClientWithServiceGuest")]
        // [AllowAnonymous]
        // public async Task<IActionResult> GetAllClientWithServiceAsync()
        // {
        //     var response = await clientHomeService.GetAllClientWithServiceAsync();
        //     return StatusCode((int)response.StatusCode, response);
        // }
        
        /// <summary>
        /// Deletes a service by its ID
        /// </summary>
        /// <param name="serviceId">The ID of the service to delete</param>
        /// <returns>Result indicating success or failure</returns>
        [HttpDelete("DeleteServiceTest/{serviceId}")]
        [AllowAnonymous]

        public async Task<IActionResult> DeleteServiceTest(int serviceId)
        {
            var response = await clientHomeService.DeleteService(serviceId);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Deletes a category by its ID
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete</param>
        /// <returns>Result indicating success or failure</returns>
        [HttpDelete("DeleteCategoryTest/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCategoryTest(int categoryId)
        {
            var response = await clientHomeService.DeleteCategory(categoryId);
            return StatusCode((int)response.StatusCode, response);
        }
        
        
        [HttpDelete("DeleteJobTest/{jobId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteJobTest(int jobId)
        {
            var response = await clientHomeService.DeleteJob(jobId);
            return StatusCode((int)response.StatusCode, response);

        }
        
        [HttpDelete("DeleteJobServiceTest/{jobId}/{serviceId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteJobServiceTest(int jobId,int serviceId)
        {
            var response = await clientHomeService.DeleteJobService(jobId,serviceId);
            return StatusCode((int)response.StatusCode, response);
        }
        
        /// <summary>
        /// Deletes an offer by its ID
        /// </summary>
        /// <param name="offerId">The ID of the offer to delete</param>
        /// <returns>Result indicating success or failure</returns>
        [HttpDelete("DeleteOfferTest/{offerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOfferTest(int offerId)
        {
            var response = await clientHomeService.DeleteOffer(offerId);
            return StatusCode((int)response.StatusCode, response);

        }
        
        
    }
}
