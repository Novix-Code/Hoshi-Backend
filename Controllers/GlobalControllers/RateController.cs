using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;
using Hoshi.Models.GlobalModels;
using Microsoft.AspNetCore.Authorization;
using Hoshi.Repositories.RatesService;
using Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs;

namespace Hoshi.Controllers.GlobalControllers.RateControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RateController : GenericFSPController<
        HoshiDbContext, 
        Rate, 
        RateGetDTO, 
        RatePostDTO, 
        RatePutDTO>
    {
        private readonly IRateService rateService;
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
,
            IRateService rateService) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

            includes = [
                $"{nameof(Rate.Client)}",
                $"{nameof(Rate.Worker)}",
            ];
            this.rateService = rateService;
        }



        public override async Task<IActionResult> Add(RatePostDTO postDTO)
        {
            var baseResponse = await base.Add(postDTO);

            if (postDTO.FromClient)
                await rateService.AddRateForWorker(postDTO);
            else 
               await rateService.AddRateForClient(postDTO);

            return baseResponse;
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<RatePostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

    }
}
