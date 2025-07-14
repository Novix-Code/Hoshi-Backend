using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs
{

    public class WorkerPaymentHistroyGetDTO : TimestampedModel
    {
        public string BillImageURL { get; set; }
        public bool IsApproved { get; set; } = false;

        public int WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }
    }
}
