using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CustomerGrowthRateDTOs
{
    public class CustomerGrowthRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public int? ClientsValue { get; set; }
        public int? WorkersValue { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
