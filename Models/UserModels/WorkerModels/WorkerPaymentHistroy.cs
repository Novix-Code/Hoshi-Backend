using GenericCRUDLibrary.CustomAttributes;

namespace Hoshi.Models.UserModels.WorkerModels
{

    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    public class WorkerPaymentHistroy
    {
        public string BillImageURL { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;

        public int WorkerId { get; set; }
        public User? Worker { get; set; }
    }
}
