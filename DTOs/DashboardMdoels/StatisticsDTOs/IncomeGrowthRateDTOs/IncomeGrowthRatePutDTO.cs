using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.IncomeGrowthRateDTOs
{
    public class IncomeGrowthRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public double? GrowthValue { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
