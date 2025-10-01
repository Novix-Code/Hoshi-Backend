using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs;
using Hoshi.Models.DashboardModels;
using Microsoft.AspNetCore.Authorization;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;

namespace Hoshi.Controllers.DashboardControllers.TermsAndCondetionsControllers
{
    [Authorize]
    //[NonController]
    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class TermsAndCondetionsController : GenericFSPController<
        HoshiDbContext, 
        TermsAndCondetions, 
        TermsAndCondetionsGetDTO, 
        TermsAndCondetionsPostDTO, 
        TermsAndCondetionsPutDTO>
    {
        public TermsAndCondetionsController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                TermsAndCondetions, 
                TermsAndCondetionsGetDTO, 
                TermsAndCondetionsPostDTO, 
                TermsAndCondetionsPutDTO> genericCRUDService,
            IGenericFSPService<
                HoshiDbContext, 
                TermsAndCondetions, 
                TermsAndCondetionsGetDTO> genericFSPService
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
        }

        [NonAction]
        public override Task<IActionResult> Add(TermsAndCondetionsPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<TermsAndCondetionsPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
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
