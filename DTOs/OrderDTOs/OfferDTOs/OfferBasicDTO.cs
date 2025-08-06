using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.UserDTOs.UserDTOs;

namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class OfferBasicDTO : TimestampedModel
    {
        public double OfferedPrice { get; set; }
        public string Note { get; set; }
    }
}
