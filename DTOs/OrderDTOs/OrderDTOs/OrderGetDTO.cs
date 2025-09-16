using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;

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


        public string OrderStatus { get; set; }

        public UserBasicDTO? Client { get; set; }

        public UserBasicDTO? Worker { get; set; }

        public CityGetDTO? City { get; set; }

        public ServiceBasicDTO? Service { get; set; }

        public PromotionGetDTO? AppliedPromotion { get; set; }

        public List<OrderImageGetDTO>? OrderImages { get; set; }

        public List<OrderVisitGetDTO>? OrderVisits { get; set; }

        public List<OrderStatusHistoryGetDTO>? OrderStatusHistory { get; set; }
    }
}
