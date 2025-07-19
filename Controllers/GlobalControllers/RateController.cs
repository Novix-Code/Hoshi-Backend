using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Controllers.GlobalControllers.RateControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RateController : GenericFSPController<
        HoshiDbContext, 
        Rate, 
        RateGetDTO, 
        RatePostDTO, 
        RatePutDTO>
    {
        public RateController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Rate, 
                RateGetDTO, 
                RatePostDTO, 
                RatePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Rate, 
                RateGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Rate.Client)}",
				$"{nameof(Rate.Worker)}",
			];
        }
    }
}
