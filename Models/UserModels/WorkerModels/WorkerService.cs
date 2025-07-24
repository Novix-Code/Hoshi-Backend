using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [Index(nameof(WorkerId), nameof(ServiceId), IsUnique = true)]
    public class WorkerService : TimestampedModel
    {
        public int WorkerId { get; set; }
        public User? Worker { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
