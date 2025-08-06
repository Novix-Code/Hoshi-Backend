using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;

namespace Hoshi.Repositories.PromotionService
{
    public interface IPromotionService
    {
        Task<ResultDTO<PromotionGetDTO>> AddPromotion(PromotionPostDTO postDTO);
        Task<ResultDTO<PromotionGetDTO>> UpdatePromotion(PromotionPutDTO putDTO);

    }
}
