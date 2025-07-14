using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs
{

    public class PromotionTakenGetDTO : TimestampedModel
    {
        public UserGetDTO? User { get; set; }

        public PromotionGetDTO? Promotion { get; set; }

        public OrderGetDTO? Order { get; set; }
        
        public OfferGetDTO? Offer { get; set; }
    }
}
