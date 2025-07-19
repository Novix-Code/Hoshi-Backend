using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.CategoryRequestRateDTOs
{
    public class CategoryRequestRatePostDTO 
    {
        public required double CategoryPercentage { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public required int ServiceCategoryId { get; set; }
    }
}
