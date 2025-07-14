using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.PromotionModels
{

    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    public class PromotionTaken : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PromotionId { get; set; }
        public Promotion? Promotion { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }
        
        public int OfferId { get; set; }
        public Offer? Offer { get; set; }
    }
}
