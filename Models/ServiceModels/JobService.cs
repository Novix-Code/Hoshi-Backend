using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.ServiceModels
{
    [EndpointGroupping("Admin")]
    [Index(nameof(ServiceId), nameof(JobId), IsUnique = true)]
    public class JobService : TimestampedModel
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
