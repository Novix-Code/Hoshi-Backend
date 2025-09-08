using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.Models.ServiceModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.ServiceControllers.ServiceCategoryControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceCategoryController : SoftDeleteGenericFSPController<
        HoshiDbContext, 
        ServiceCategory, 
        ServiceCategoryGetDTO, 
        ServiceCategoryPostDTO, 
        ServiceCategoryPutDTO>
    {
        public ServiceCategoryController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                ServiceCategory, 
                ServiceCategoryGetDTO, 
                ServiceCategoryPostDTO, 
                ServiceCategoryPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                ServiceCategory, 
                ServiceCategoryGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(ServiceCategory.Services)}",
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

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(ServiceCategoryPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<ServiceCategoryPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(ServiceCategoryPutDTO putDTO)
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
