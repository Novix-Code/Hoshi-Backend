using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CategoryRequestRateDTOs
{
    public class CategoryRequestRateGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public double CategoryPercentage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ServiceCategoryGetDTO? ServiceCategory { get; set; }
    }
}
