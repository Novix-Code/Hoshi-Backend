using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.GlobalModels;

namespace Hoshi.Models.DashboardModels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class ComplaintSolvingRate : IBaseModel
    {
        public int Id { get; set; }
        public int TotalComplaints { get; set; }
        public int UnderSolvingNumber { get; set; }
        public int SolvedNumber { get; set; }
        public int NotSolvedNumber { get; set; }
        public bool ForClient { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ComplaintTypeId { get; set; }
        public ComplaintType? ComplaintType { get; set; }
    }
}
