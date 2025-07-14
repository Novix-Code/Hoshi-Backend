using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerRejectionDTOs
{
    public class WorkerRejectionPostDTO 
    {
        public required string RejectionReason { get; set; }

        public required int WorkerId { get; set; }
    }
}
