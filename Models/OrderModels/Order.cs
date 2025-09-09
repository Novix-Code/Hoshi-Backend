using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.ServiceModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.OrderModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.AddList)]
    [EndpointGroupping("Client", ControllerAction.Add)]
    [EndpointGroupping("Client", ControllerAction.Update)]
    [CreateRepoPattern("ClientOrderService")]
    [CreateRepoPattern("WorkerOrderService")]
    public class Order : TimestampedModel
    {
        public string Description { get; set; } = string.Empty;
        public double ProposalPrice { get; set; }

        public string Location { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime ServicingDateTime { get; set; }


        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public double? TotalClientCost { get; set; }

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public double? TotalWorkerCost { get; set; }


        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public string OrderStatus { get; set; } = Enums.OrderStatus.Published.ToString();

        public int ClientId { get; set; }
        public User? Client { get; set; }

        [PropNotMapped(DtoType.Post)]
        public int? WorkerId { get; set; }
        public User? Worker { get; set; }

        public int CityId { get; set; }
        public City? City { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? AppliedPromotionId { get; set; }
        public Promotion? AppliedPromotion { get; set; }

        public List<OrderImage>? OrderImages { get; set; }

        [PropNotMapped(DtoType.Post)]
        public List<OrderVisit>? OrderVisits { get; set; }

        [PropNotMapped(DtoType.Post)]
        public List<OrderStatusHistory>? OrderStatusHistory { get; set; }
    }
}
