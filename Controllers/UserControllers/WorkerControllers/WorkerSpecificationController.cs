using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.UserService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Repositories.WorkerSpecificationService;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerSpecificationControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerSpecificationController : GenericJustFSPController<
        HoshiDbContext, 
        WorkerSpecification, 
        WorkerSpecificationGetDTO>
    {
        private readonly IGenericCRUDService<HoshiDbContext, WorkerSpecification, WorkerSpecificationGetDTO, WorkerSpecificationPostDTO, WorkerSpecificationPutDTO> genericCRUDService;
        private readonly IUserService userService;
        private readonly IWorkerSpecificationService specificationService;

        public WorkerSpecificationController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerSpecification, 
                WorkerSpecificationGetDTO, 
                WorkerSpecificationPostDTO, 
                WorkerSpecificationPutDTO> genericCRUDService,
            IGenericFSPService<
                HoshiDbContext, 
                WorkerSpecification, 
                WorkerSpecificationGetDTO> genericFSPService,
            IUserService userService,
            IWorkerSpecificationService specificationService
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerSpecification.User)}",
				$"{nameof(WorkerSpecification.LivingCity)}",
				$"{nameof(WorkerSpecification.Job)}",
			];
            this.genericCRUDService = genericCRUDService;
            this.userService = userService;
            this.specificationService = specificationService;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int userId)
        {
            var result = await specificationService.GetWorkerById(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(WorkerSpecificationPutDTO putDTO)
        {
            // Update Worker Personal Image, Portfolio, Services
            var result = await specificationService.UpdateWorker(putDTO);

            // If Update done successfully will update basic data
            if (result.IsSuccess)
            {
                var baseResult = await genericCRUDService.Update(
                    putDTO,
                    [
                        $"{nameof(WorkerSpecification.User)}",
                        $"{nameof(WorkerSpecification.LivingCity)}",
                        $"{nameof(WorkerSpecification.Job)}",
                    ]
                );

                // If the basic date updated successfully will call GetWorkerById to get all worker data.
                if (baseResult.IsSuccess)
                {
                    var getResult = await specificationService.GetWorkerById(baseResult.Data!.User!.Id);

                    return StatusCode(getResult.StatusCode, getResult);
                }
                else
                    return StatusCode(baseResult.StatusCode, baseResult);
            }
            else
                // else will return the result
                return StatusCode(result.StatusCode, result);
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
