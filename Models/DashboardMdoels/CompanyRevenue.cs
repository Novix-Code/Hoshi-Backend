using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.OrderModels;

namespace Hoshi.Models.DashboardMdoels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class CompanyRevenue : IBaseModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
