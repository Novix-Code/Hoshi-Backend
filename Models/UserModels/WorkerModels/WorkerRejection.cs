using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.WorkerModels
{

    public class WorkerRejection : TimestampedModel
    {
        public string RejectionReason { get; set; } = string.Empty;

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
