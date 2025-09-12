using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.RateDTOs
{
    public class RatePostDTO 
    {
        public string Description { get; set; } = string.Empty;
        public required double RateValue { get; set; }
        public required bool FromClient { get; set; }

        public required int ClientId { get; set; }

        public required int WorkerId { get; set; }
    }
}
