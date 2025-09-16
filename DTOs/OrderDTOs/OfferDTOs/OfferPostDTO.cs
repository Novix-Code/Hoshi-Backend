using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class OfferPostDTO 
    {
        public required double OfferedPrice { get; set; }
        public string Note { get; set; } = "لا توجد ملاحظات";

        public required int WorkerId { get; set; }

        public required int OrderId { get; set; }

    }
}
