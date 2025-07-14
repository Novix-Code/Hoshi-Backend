using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Controllers.UserControllers.WorkerControllers.WorkerPortfolioControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Worker")]
    public class WorkerPortfolioController : GenericFSPController<
        HoshiDbContext, 
        WorkerPortfolio, 
        WorkerPortfolioGetDTO, 
        WorkerPortfolioPostDTO, 
        WorkerPortfolioPutDTO>
    {
        public WorkerPortfolioController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                WorkerPortfolio, 
                WorkerPortfolioGetDTO, 
                WorkerPortfolioPostDTO, 
                WorkerPortfolioPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                WorkerPortfolio, 
                WorkerPortfolioGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(WorkerPortfolio.Worker)}",
			];
        }
    }
}
