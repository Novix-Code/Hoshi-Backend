using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs
{
    public class WorkerWalletHistoryPostDTO 
    {
        public required string Title { get; set; }
        public required double Value { get; set; }
        public required bool IsIncome { get; set; }

        public required int WorkerWalletId { get; set; }
    }
}
