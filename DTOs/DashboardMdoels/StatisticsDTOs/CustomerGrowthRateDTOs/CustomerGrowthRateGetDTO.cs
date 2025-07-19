using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.CustomerGrowthRateDTOs
{
    public class CustomerGrowthRateGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public int ClientsValue { get; set; }
        public int WorkersValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
