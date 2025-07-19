using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerSpecificationControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerSpecificationController : GenericController<
        HoshiDbContext, 
        WorkerSpecification, 
        WorkerSpecificationGetDTO, 
        WorkerSpecificationPostDTO, 
        WorkerSpecificationPutDTO>
    {
        public WorkerSpecificationController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerSpecification, 
                WorkerSpecificationGetDTO, 
                WorkerSpecificationPostDTO, 
                WorkerSpecificationPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerSpecification.User)}",
				$"{nameof(WorkerSpecification.LivingCity)}",
				$"{nameof(WorkerSpecification.Job)}.{nameof(Job.Services)}",
				$"{nameof(WorkerSpecification.Services)}.{nameof(Service.ServiceCategory)}",
			];
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
