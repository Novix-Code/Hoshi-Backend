using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.IncomeGrowthRateDTOs
{
    public class IncomeGrowthRatePostDTO 
    {
        public required double GrowthValue { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
