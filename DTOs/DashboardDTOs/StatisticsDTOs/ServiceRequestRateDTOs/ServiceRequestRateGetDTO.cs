using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ServiceRequestRateDTOs
{
    public class ServiceRequestRateGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public double ServicePercentage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ServiceGetDTO? Service { get; set; }
    }
}
