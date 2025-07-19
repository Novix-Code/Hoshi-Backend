using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [UseFSPController]
    [EndpointGroupping("Worker")]
    public class WorkerRejection : TimestampedModel
    {
        public string RejectionReason { get; set; } = string.Empty;

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
