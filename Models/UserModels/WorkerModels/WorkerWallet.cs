using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Worker")]
    public class WorkerWallet : TimestampedModel
    {
        public double Balance { get; set; } = 0.0;
        public bool HitLimit { get; set; } = false;

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
