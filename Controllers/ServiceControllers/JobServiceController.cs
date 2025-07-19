using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.JobServiceDTOs;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Controllers.ServiceControllers.JobServiceControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class JobServiceController : GenericController<
        HoshiDbContext, 
        JobService, 
        JobServiceGetDTO, 
        JobServicePostDTO, 
        JobServicePutDTO>
    {
        public JobServiceController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                JobService, 
                JobServiceGetDTO, 
                JobServicePostDTO, 
                JobServicePutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(JobService.Job)}.{nameof(Job.Services)}",
				$"{nameof(JobService.Service)}.{nameof(Service.ServiceCategory)}",
			];
        }
    }
}
