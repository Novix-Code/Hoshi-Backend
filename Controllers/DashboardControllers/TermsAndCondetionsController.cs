using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs;
using Hoshi.Models.DashboardModels;

namespace Hoshi.Controllers.DashboardControllers.TermsAndCondetionsControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Admin")]
    public class TermsAndCondetionsController : GenericController<
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
                TermsAndCondetionsPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
        }
    }
}
