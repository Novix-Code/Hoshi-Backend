using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Models.DashboardModels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class ServiceRequestRate : IBaseModel
    {
        public int Id { get; set; }
        public double ServicePercentage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
