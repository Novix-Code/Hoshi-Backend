using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs
{

    public class WorkerPaymentHistroyPostDTO 
    {
        public required string BillImageURL { get; set; }
        public required bool IsApproved { get; set; } = false;

        public required int WorkerId { get; set; }
    }
}
