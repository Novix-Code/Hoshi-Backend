using Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsValueDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsValueDTOs
{
    public class NumericalStatisticsValueGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public double CurrentValue { get; set; }
        public double? PercentageValue { get; set; }
        public bool IsIncreased { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? LastValueId { get; set; }
        public NumericalStatisticsValueGetDTO? LastValue { get; set; }
    }
}
