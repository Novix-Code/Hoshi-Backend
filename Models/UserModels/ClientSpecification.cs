using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels
{
    [Index(nameof(UserId), IsUnique = true)]
    [NoAction(ControllerAction.Delete)]
    public class ClientSpecification : TimestampedModel
    {
        public string ImageURL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;
        public double Balance { get; set; } = 0.0;
        public double Indebtedness { get; set; } = 0.0;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int LivingCityId { get; set; }
        public City? LivingCity { get; set; }
    }
}
