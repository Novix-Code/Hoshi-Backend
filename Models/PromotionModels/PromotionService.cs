using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.PromotionModels
{

    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    [Index(nameof(ServiceId), nameof(PromotionId), IsUnique = true)]
    public class PromotionService : IBaseModel
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int PromotionId { get; set; }
        public Promotion? Promotion { get; set; }

    }
}
