using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.CustomerGrowthRateDTOs
{
    public class CustomerGrowthRatePostDTO 
    {
        public required int ClientsValue { get; set; }
        public required int WorkersValue { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
