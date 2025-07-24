using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.StatisticsDTOs.ComplaintSolvingRateDTOs
{
    public class ComplaintSolvingRatePostDTO 
    {
        public required int TotalComplaints { get; set; }
        public required int UnderSolvingNumber { get; set; }
        public required int SolvedNumber { get; set; }
        public required int NotSolvedNumber { get; set; }
        public required bool ForClient { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public required int ComplaintTypeId { get; set; }
    }
}
