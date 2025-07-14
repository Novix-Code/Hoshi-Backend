using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs
{

    public class WorkerPaymentHistroyPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? BillImageURL { get; set; }
        public bool? IsApproved { get; set; } = false;

        public int? WorkerId { get; set; }
    }
}
