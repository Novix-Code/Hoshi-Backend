using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [UseFSPController]
    [EndpointGroupping("Worker")]
    public class WorkerPortfolio : TimestampedModel
    {
        public string FileURL { get; set; } = string.Empty;

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
