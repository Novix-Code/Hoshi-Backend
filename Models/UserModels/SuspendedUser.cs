using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    [UseFSPController]
    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    [EndpointGroupping("Admin", ControllerAction.Delete)]
    public class SuspendedUser : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int SuspendReasonId { get; set; }
        public SuspendReason? SuspendReason { get; set; }
    }
}
