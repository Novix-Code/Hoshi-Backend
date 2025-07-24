using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsValueDTOs
{
    public class NumericalStatisticsValuePostDTO 
    {
        public required double CurrentValue { get; set; }
        public double? PercentageValue { get; set; }
        public required bool IsIncreased { get; set; } = true;
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? LastValueId { get; set; }
    }
}
