using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardMdoels.TermsAndCondetionsDTOs;
using Hoshi.Models.DashboardMdoels;

namespace Hoshi.Controllers.DashboardMdoels.TermsAndCondetionsControllers
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
