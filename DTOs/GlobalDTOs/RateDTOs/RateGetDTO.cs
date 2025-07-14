using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.RateDTOs
{
    public class RateGetDTO : TimestampedModel
    {
        public string Description { get; set; }
        public double RateValue { get; set; }
        public bool FromClient { get; set; }

        public int ClientId { get; set; }
        public UserGetDTO? Client { get; set; }

        public int WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }
    }
}
