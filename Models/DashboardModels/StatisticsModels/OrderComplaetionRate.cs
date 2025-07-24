using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Models.DashboardModels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class OrderComplaetionRate : IBaseModel
    {
        public int Id { get; set; }
        public int CancelledValue { get; set; }
        public int AssignedValue { get; set; }
        public int CompletedValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }
    }
}
