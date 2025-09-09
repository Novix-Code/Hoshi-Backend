using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.OrderModels
{
    [UseFSPController]
    [NoAction(ControllerAction.AddList)]
    [EndpointGroupping("Worker", ControllerAction.Add)]
    [EndpointGroupping("Worker", ControllerAction.Update)]
    [CreateRepoPattern("ClientOfferService")]
    [CreateRepoPattern("WorkerOfferService")]
    public class Offer : TimestampedModel, ISoftDelete
    {
        public double OfferedPrice { get; set; }
        public string Note { get; set; } = string.Empty;

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public DateTime? AcceptedDateTime { get; set; }

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public bool? IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }


        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public string OfferStatus { get; set; } = Enums.OfferStatus.Waitting.ToString();

        public int WorkerId { get; set; }
        public User? Worker { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int? AppliedPromotionId { get; set; }
        public Promotion? AppliedPromotion { get; set; }

    }
}
