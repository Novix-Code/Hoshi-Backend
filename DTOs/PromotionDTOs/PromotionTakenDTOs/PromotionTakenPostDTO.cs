using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs
{

    public class PromotionTakenPostDTO 
    {
        public required int UserId { get; set; }

        public required int PromotionId { get; set; }

        public required int OrderId { get; set; }
        
        public required int OfferId { get; set; }
    }
}
