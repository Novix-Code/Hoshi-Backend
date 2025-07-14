using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.ServiceModels
{
    public class JobService : TimestampedModel
    {
        public int JobId { get; set; }
        public Job? Job { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
