using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.CategoryRequestRateDTOs
{
    public class CategoryRequestRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public double? CategoryPercentage { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public int? ServiceCategoryId { get; set; }
    }
}
