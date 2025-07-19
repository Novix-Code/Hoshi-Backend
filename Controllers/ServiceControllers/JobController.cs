using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Controllers.ServiceControllers.JobControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class JobController : SoftDeleteGenericController<
        HoshiDbContext, 
        Job, 
        JobGetDTO, 
        JobPostDTO, 
        JobPutDTO>
    {
        public JobController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Job, 
                JobGetDTO, 
                JobPostDTO, 
                JobPutDTO> genericCRUDService
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(Job.Services)}.{nameof(Service.ServiceCategory)}",
			];
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(JobPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<JobPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(JobPutDTO putDTO)
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
