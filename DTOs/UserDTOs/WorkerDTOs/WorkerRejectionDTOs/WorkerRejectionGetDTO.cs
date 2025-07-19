using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerRejectionDTOs
{
    public class WorkerRejectionGetDTO : TimestampedModel
    {
        public string RejectionReason { get; set; }

        public int WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }
    }
}
