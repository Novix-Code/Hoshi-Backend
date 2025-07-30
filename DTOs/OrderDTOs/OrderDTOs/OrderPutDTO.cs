using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Description { get; set; }
        public double? ProposalPrice { get; set; }

        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime? ServicingDateTime { get; set; }

        public int? ClientId { get; set; }

        public int? WorkerId { get; set; }

        public int? CityId { get; set; }

        public int? ServiceId { get; set; }

        public int? AppliedPromotionId { get; set; }
    }
}
