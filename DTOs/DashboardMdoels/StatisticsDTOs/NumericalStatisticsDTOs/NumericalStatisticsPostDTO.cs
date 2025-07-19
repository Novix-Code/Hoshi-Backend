using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.NumericalStatisticsDTOs
{

    public class NumericalStatisticsPostDTO 
    {
        public required string Title { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
