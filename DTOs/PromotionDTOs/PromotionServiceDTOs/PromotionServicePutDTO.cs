using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.PromotionDTOs.PromotionServiceDTOs
{

    public class PromotionServicePutDTO  : IBaseModel
    {
        public int Id { get; set; }

        public int? ServiceId { get; set; }

        public int? PromotionId { get; set; }

    }
}
