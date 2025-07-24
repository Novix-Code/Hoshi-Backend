using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.OrderComplaetionRateDTOs
{
    public class OrderComplaetionRatePostDTO 
    {
        public required int CancelledValue { get; set; }
        public required int AssignedValue { get; set; }
        public required int CompletedValue { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public required int ServiceCategoryId { get; set; }
    }
}
