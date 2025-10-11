using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.IncomeGrowthRateDTOs
{
    public class IncomeGrowthRateGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public double GrowthValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
