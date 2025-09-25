using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.Repositories.ClientHomeService;
using Hoshi.Repositories.ServiceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.ServiceControllers.ServiceControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        Service, 
        ServiceGetDTO, 
        ServicePostDTO, 
        ServicePutDTO>
    {
		private readonly IClientHomeService clientHomeService;
		private readonly IServiceService serviceService;
        public ServiceController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Service, 
                ServiceGetDTO, 
                ServicePostDTO, 
                ServicePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Service, 
                ServiceGetDTO> genericFSPService,
			IClientHomeService clientHomeService,
			IServiceService serviceService
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Service.ServiceCategory)}",
			];
			this.clientHomeService = clientHomeService;
			this.serviceService = serviceService;
        }

        [HttpPost("Search")]
        [EndpointGroupName("Client")]
        public async Task<IActionResult> ServiceSearch([FromForm] string serviceName)
        {
            var response = await serviceService.searchServiceAsyn(serviceName);
            return StatusCode((int)response.StatusCode, response);

        }
        
        [EndpointGroupName("Client")]
        public override Task<IActionResult> GetById(int id)
        {
            return base.GetById(id);
        }
        
        [EndpointGroupName("Client")]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }

        [EndpointGroupName("Admin")]
        public override async Task<IActionResult> Add(ServicePostDTO postDTO)
        {
            var result = await serviceService.AddService(postDTO);
            return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Admin")]
        public override async Task<IActionResult> Update(ServicePutDTO putDTO)
        {
            var result = await serviceService.UpdateService(putDTO);
            return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Restore(int id)
        {
            return base.Restore(id);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<ServicePostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override IActionResult Pagination([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] bool ascending = true)
        {
            return base.Pagination(pageNumber, pageSize, ascending);
        }

        [NonAction]
        public override IActionResult PaginationFilteredSearch(PaginationFilteredSearchDTO paginationFilteredSearchDTO)
        {
            return base.PaginationFilteredSearch(paginationFilteredSearchDTO);
        }
        
    }
}
