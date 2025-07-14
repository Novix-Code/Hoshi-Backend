using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.WorkerModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Worker")]
    public class WorkerWalletHistory : TimestampedModel
    {
        public string Title { get; set; } = string.Empty;
        public double Value { get; set; }
        public bool IsIncome { get; set; }

        public int WorkerWalletId { get; set; }
        public WorkerWallet? WorkerWallet { get; set; }
    }
}
