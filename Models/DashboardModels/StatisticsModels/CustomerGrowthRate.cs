using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.DashboardModels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class CustomerGrowthRate : IBaseModel
    {
        public int Id { get; set; }
        public int ClientsValue { get; set; }
        public int WorkersValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
