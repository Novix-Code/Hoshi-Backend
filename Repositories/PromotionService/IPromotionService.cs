using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;

namespace Hoshi.Repositories.PromotionService
{
    /// <summary>
    /// Promotion CRUD helpers for creating and updating promotions with image handling.
    /// </summary>
    public interface IPromotionService
    {
        /// <summary>
        /// Create a promotion and persist its image via FileService.
        /// </summary>
        Task<ResultDTO<PromotionGetDTO>> AddPromotion(PromotionPostDTO postDTO);
        /// <summary>
        /// Update a promotion and optionally replace its image (deletes the old image first).
        /// </summary>
        Task<ResultDTO<PromotionGetDTO>> UpdatePromotion(PromotionPutDTO putDTO);

    }
}
