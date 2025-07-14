using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs
{

    public class PromotionTakenPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? UserId { get; set; }

        public int? PromotionId { get; set; }

        public int? OrderId { get; set; }
        
        public int? OfferId { get; set; }
    }
}
