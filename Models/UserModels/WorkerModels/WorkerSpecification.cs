using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [Index(nameof(UserId), IsUnique = true)]
    [NoAction(ControllerAction.Delete)]
    [EndpointGroupping("Worker")]
    public class WorkerSpecification : TimestampedModel
    {
        public string Bio { get; set; } = string.Empty;
        public string IdentityImageURL { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;

        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;

        public bool IsCompany { get; set; } = false;
        public bool? IsApproved { get; set; } = null;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int LivingCityId { get; set; } = 1;
        public City? LivingCity { get; set; }

        public int JobId { get; set; } = 1;
        public Job? Job { get; set; }
    }
}
