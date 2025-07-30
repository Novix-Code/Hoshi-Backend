using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class OfferPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? OfferedPrice { get; set; }
        public string? Note { get; set; }
    }
}
