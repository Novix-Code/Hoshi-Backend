using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.ServiceRequestRateDTOs
{
    public class ServiceRequestRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public double? ServicePercentage { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public int? ServiceId { get; set; }
    }
}
