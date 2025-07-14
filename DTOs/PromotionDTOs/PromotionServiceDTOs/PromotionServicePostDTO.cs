using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.PromotionDTOs.PromotionServiceDTOs
{

    public class PromotionServicePostDTO 
    {

        public required int ServiceId { get; set; }

        public required int PromotionId { get; set; }

    }
}
