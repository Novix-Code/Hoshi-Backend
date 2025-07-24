using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.DashboardModels.StatisticsModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class NumericalStatisticsValue : IBaseModel
    {
        public int Id { get; set; }
        public double CurrentValue { get; set; }
        public double? PercentageValue { get; set; }
        public bool IsIncreased { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? LastValueId { get; set; }
        public NumericalStatisticsValue? LastValue { get; set; }
    }
}
