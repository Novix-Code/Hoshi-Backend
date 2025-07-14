using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerRejectionDTOs
{
    public class WorkerRejectionPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? RejectionReason { get; set; }

        public int? WorkerId { get; set; }
    }
}
