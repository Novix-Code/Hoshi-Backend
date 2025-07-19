using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.RateDTOs
{
    public class RatePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Description { get; set; }
        public double? RateValue { get; set; }
        public bool? FromClient { get; set; }

        public int? ClientId { get; set; }

        public int? WorkerId { get; set; }
    }
}
