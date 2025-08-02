using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.UserDTOs.UserDTOs;

namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationGetDTO : TimestampedModel
    {
        public string Address { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;
        public double Balance { get; set; } = 0.0;
        public double Indebtedness { get; set; } = 0.0;

        public UserGetDTO? User { get; set; }
    }
}
