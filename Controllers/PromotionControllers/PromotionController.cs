using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.Models.PromotionModels;

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
        public PromotionController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Promotion, 
                PromotionGetDTO, 
                PromotionPostDTO, 
                PromotionPutDTO> genericCRUDService
        ) : base(mapper, genericCRUDService)
        {
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(PromotionPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<PromotionPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(PromotionPutDTO putDTO)
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
