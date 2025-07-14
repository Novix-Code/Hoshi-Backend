using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.PromotionDTOs.PromotionServiceDTOs
{

    public class PromotionServiceGetDTO : IBaseModel
    {
        public int Id { get; set; }

        public ServiceGetDTO? Service { get; set; }

        public PromotionGetDTO? Promotion { get; set; }

    }
}
