using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetDTO : TimestampedModel
    {
        public string Description { get; set; }
        public double ProposalPrice { get; set; }

        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime ServicingDateTime { get; set; }


        public double? TotalClientCost { get; set; }

        public double? TotalWorkerCost { get; set; }


        public OrderStatus OrderStatus { get; set; } = OrderStatus.Published;

        public int ClientId { get; set; }
        public UserGetDTO? Client { get; set; }

        public int? WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }

        public CityGetDTO? City { get; set; }

        public ServiceGetDTO? Service { get; set; }

        public int? AppliedPromotionId { get; set; }
        public PromotionGetDTO? AppliedPromotion { get; set; }

        public List<OrderImageGetDTO>? OrderImages { get; set; }

        public List<OrderVisitGetDTO>? OrderVisits { get; set; }

        public List<OrderStatusHistoryGetDTO>? OrderStatusHistory { get; set; }
    }
}
