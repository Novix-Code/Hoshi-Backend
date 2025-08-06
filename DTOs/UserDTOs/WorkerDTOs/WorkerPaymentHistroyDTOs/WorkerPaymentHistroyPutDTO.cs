using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs
{

    public class WorkerPaymentHistroyPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public IFormFile? BillImage { get; set; }
        public bool? IsApproved { get; set; } = false;
    }
}
