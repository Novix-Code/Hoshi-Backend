using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs
{
    public class WorkerWalletPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? Balance { get; set; } = 0.0;
        public bool? HitLimit { get; set; } = false;

        public int? WorkerId { get; set; }
    }
}
