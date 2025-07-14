using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerRejectionDTOs;
using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerRejectionControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerRejectionController : GenericFSPController<
        HoshiDbContext, 
        WorkerRejection, 
        WorkerRejectionGetDTO, 
        WorkerRejectionPostDTO, 
        WorkerRejectionPutDTO>
    {
        public WorkerRejectionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerRejection, 
                WorkerRejectionGetDTO, 
                WorkerRejectionPostDTO, 
                WorkerRejectionPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                WorkerRejection, 
                WorkerRejectionGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerRejection.Worker)}",
			];
        }
    }
}
