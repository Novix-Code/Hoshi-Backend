using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ComplaintSolvingRateDTOs
{
    public class ComplaintSolvingRatePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public int? TotalComplaints { get; set; }
        public int? UnderSolvingNumber { get; set; }
        public int? SolvedNumber { get; set; }
        public int? NotSolvedNumber { get; set; }
        public bool? ForClient { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public int? ComplaintTypeId { get; set; }
    }
}
