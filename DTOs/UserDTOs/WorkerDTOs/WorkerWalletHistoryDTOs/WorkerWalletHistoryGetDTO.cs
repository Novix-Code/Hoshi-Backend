using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs
{
    public class WorkerWalletHistoryGetDTO : TimestampedModel
    {
        public string Title { get; set; }
        public double Value { get; set; }
        public bool IsIncome { get; set; }

        public WorkerWalletGetDTO? WorkerWallet { get; set; }
    }
}
