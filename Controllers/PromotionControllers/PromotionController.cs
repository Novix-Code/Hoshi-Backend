using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.PromotionService;

namespace Hoshi.Controllers.PromotionControllers.PromotionControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : SoftDeleteGenericController<
        HoshiDbContext, 
        Promotion, 
        PromotionGetDTO, 
        PromotionPostDTO, 
        PromotionPutDTO>
    {
        private readonly IPromotionService promotionService;

        public PromotionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Promotion, 
                PromotionGetDTO, 
                PromotionPostDTO, 
                PromotionPutDTO> genericCRUDService,
            IPromotionService promotionService
        ) : base(mapper, genericCRUDService)
        {
            this.promotionService = promotionService;
        }

        [EndpointGroupName("Admin")]
        public override async Task<IActionResult> Add(PromotionPostDTO postDTO)
        {
            var result = await promotionService.AddPromotion(postDTO);
            return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Admin")]
        public override async Task<IActionResult> Update(PromotionPutDTO putDTO)
        {
            var result = await promotionService.UpdatePromotion(putDTO);
            return StatusCode(result.StatusCode, result);
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

        [NonAction]
        public override Task<IActionResult> AddList(List<PromotionPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }
    }
}
