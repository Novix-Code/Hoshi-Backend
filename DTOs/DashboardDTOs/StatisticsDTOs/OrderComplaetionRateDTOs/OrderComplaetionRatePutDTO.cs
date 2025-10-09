using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.OrderComplaetionRateDTOs
{
    public class OrderComplaetionRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public int? CancelledValue { get; set; }
        public int? AssignedValue { get; set; }
        public int? CompletedValue { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ServiceCategoryId { get; set; }
    }
}
