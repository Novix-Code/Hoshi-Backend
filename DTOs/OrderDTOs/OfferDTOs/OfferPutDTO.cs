using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class OfferPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? OfferedPrice { get; set; }
        public string? Note { get; set; }


        public bool? IsDeleted { get; set; }



        public int? WorkerId { get; set; }

        public int? OrderId { get; set; }

        public int? AppliedPromotionId { get; set; }

    }
}
