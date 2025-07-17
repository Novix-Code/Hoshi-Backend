using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Repositories.ServiceService;

using Hoshi.Repositories.ClientHomeService;

using Hoshi.Models.ServiceModels;

namespace Hoshi.Controllers.ServiceControllers.ServiceControllers
{

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
        [NonAction]
        public override IActionResult FilteredSearch(List<FilteredSearchDTO> filters)
        {
            return base.FilteredSearch(filters);    

        }
        [EndpointGroupName("Client")]
        public override async Task<IActionResult> GetById(int id)
        {
            var response = await clientHomeService.GetByIdServiceAsync(id);
            return StatusCode((int)response.StatusCode,response);
        }
        [EndpointGroupName("Client")]
        public override async Task<IActionResult> GetAll()
        {
            var response  = await clientHomeService.GetAllServiceAsync();
            return StatusCode((int)response.StatusCode, response);
        }



        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(ServicePostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<ServicePostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(ServicePutDTO putDTO)
        {
            return base.Update(putDTO);
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
    }
}
