using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    public class Rate : TimestampedModel
    {
        public string Description { get; set; } = string.Empty;
        public double RateValue { get; set; }
        public bool FromClient { get; set; }

        public int ClientId { get; set; }
        public User? Client { get; set; }

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
