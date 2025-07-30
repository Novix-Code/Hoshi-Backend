using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class OfferGetDTO : TimestampedModel, ISoftDelete
    {
        public double OfferedPrice { get; set; }
        public string Note { get; set; }

        public DateTime? AcceptedDateTime { get; set; }

        public bool? IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }


        public OfferStatus OfferStatus { get; set; } = OfferStatus.Waitting;

        public UserGetDTO? Worker { get; set; }

        public OrderGetDTO? Order { get; set; }

        public PromotionGetDTO? AppliedPromotion { get; set; }

    }
}
