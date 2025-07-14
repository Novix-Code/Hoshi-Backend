using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs
{
    public class WorkerWalletHistoryPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Title { get; set; }
        public double? Value { get; set; }
        public bool? IsIncome { get; set; }

        public int? WorkerWalletId { get; set; }
    }
}
