using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ServiceRequestRateDTOs
{
    public class ServiceRequestRatePostDTO 
    {
        public required double ServicePercentage { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required int ServiceId { get; set; }
    }
}
