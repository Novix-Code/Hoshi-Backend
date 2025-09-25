using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;

namespace Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs
{

    public class PromotionTakenGetDTO : TimestampedModel
    {
        public UserBasicDTO? User { get; set; }

        public PromotionGetDTO? Promotion { get; set; }

        public OrderGetAllDto? Order { get; set; }
        
        public OfferBasicDTO? Offer { get; set; }
    }
}
