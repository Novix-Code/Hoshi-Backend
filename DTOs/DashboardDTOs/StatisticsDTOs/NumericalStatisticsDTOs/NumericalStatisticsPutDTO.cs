using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.NumericalStatisticsDTOs
{

    public class NumericalStatisticsPutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
