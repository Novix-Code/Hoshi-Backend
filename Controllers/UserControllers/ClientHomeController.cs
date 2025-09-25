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
    }
}
