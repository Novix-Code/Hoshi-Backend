using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    [UseFSPController]
    public class SuspendedUser : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int SuspendReasonId { get; set; }
        public SuspendReason? SuspendReason { get; set; }
    }
}
