using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CategoryRequestRateDTOs
{
    public class CategoryRequestRatePostDTO 
    {
        public required double CategoryPercentage { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required int ServiceCategoryId { get; set; }
    }
}
