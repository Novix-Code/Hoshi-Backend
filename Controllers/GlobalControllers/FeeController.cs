using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using Hoshi.Models.ServiceModels;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.FeeDTOs;
using Hoshi.Models.GlobalModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.GlobalControllers.FeeControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class FeeController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        Fee, 
        FeeGetDTO, 
        FeePostDTO, 
        FeePutDTO>
    {
        public FeeController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Fee, 
                FeeGetDTO, 
                FeePostDTO, 
                FeePutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Fee, 
                FeeGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Fee.Service)}.{nameof(Service.ServiceCategory)}",
			];
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
