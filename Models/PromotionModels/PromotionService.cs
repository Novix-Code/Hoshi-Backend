using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Models.PromotionModels
{

    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class PromotionService : IBaseModel
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int PromotionId { get; set; }
        public Promotion? Promotion { get; set; }

    }
}
