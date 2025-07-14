using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.DashboardMdoels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class IncomeGrowthRate : IBaseModel
    {
        public int Id { get; set; }
        public double GrowthValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
