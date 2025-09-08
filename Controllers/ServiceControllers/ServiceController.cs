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
        public override async Task<IActionResult> GetById(int id)
        {
            var response = await clientHomeService.GetClientWithServiceById(id);
            return StatusCode((int)response.StatusCode,response);
        }
        
        [EndpointGroupName("Client")]
        public override async Task<IActionResult> GetAll()
        {
            var response  = await clientHomeService.GetAllClientWithServiceAsync();
            return StatusCode((int)response.StatusCode, response);
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
        
        [HttpGet("get-services-page")]
        public async Task<IActionResult> GetServicesPageAsync()
        {
            var response = await serviceService.GetServicesPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("get-payments-page")]
        public async Task<IActionResult> GetPaymentsPageAsync()
        {
            var response = await serviceService.GetPaymentsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("get-payment-details")]
        public async Task<IActionResult> GetPaymentDetailsAsync([FromQuery ]int paymentId)
        {
            var response = await serviceService.GetPaymentDetailsAsync(paymentId);
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpPatch("add-payment")]
        public async Task<IActionResult> AddWorkerPayment(int workerId, double paymentValue)
        {
            var response = await serviceService.AddWorkerPayment(workerId, paymentValue);
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("get-complaints-page")]
        public async Task<IActionResult> GetComplaintsPageAsync()
        {
            var response = await serviceService.GetComplaintsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpGet("get-complaint-details")]
        public async Task<IActionResult> GetComplaintDetailsAsync([FromQuery ]int complaintId)
        {
            var response = await serviceService.GetComplaintDetailsAsync(complaintId);
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpPatch("complaint-response")]
        public async Task<IActionResult> ComplaintResponse(ComplaintResponseDTO complaintCreateDto)
        {
            var response = await serviceService.ComplaintResponse(complaintCreateDto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("close-complaint")]
        public async Task<IActionResult> CloseComplaintAsync([FromQuery] int complaintId)
        {
            var result = await serviceService.CloseComplaintAsync(complaintId);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpGet("get-statistic-page")]
        public async Task<IActionResult> GetStatisticPageAsync()
        {
            var response = await serviceService.GetStatisticPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
