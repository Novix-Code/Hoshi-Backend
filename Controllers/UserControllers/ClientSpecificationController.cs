using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.ClientSpecificationService;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.UserControllers.ClientSpecificationControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Client")]
    public class ClientSpecificationController : GenericJustFSPController<
        HoshiDbContext, 
        ClientSpecification, 
        ClientSpecificationGetDTO>
    {
        private readonly IClientSpecificationService specificationService;

        public ClientSpecificationController(
            IMapper mapper, 
            IGenericFSPService<
                HoshiDbContext,
                ClientSpecification,
                ClientSpecificationGetDTO> genericFSPService,
            IClientSpecificationService specificationService
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(ClientSpecification.User)}",
			];
            this.specificationService = specificationService;
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetById(int userId)
        {
            var result = await specificationService.GetById(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ClientSpecificationPutDTO putDTO)
        {
            var result = await specificationService.Update(putDTO);
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
