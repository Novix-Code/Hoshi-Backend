using Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardMdoels.StatisticsDTOs.ComplaintSolvingRateDTOs
{
    public class ComplaintSolvingRateGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public int TotalComplaints { get; set; }
        public int UnderSolvingNumber { get; set; }
        public int SolvedNumber { get; set; }
        public int NotSolvedNumber { get; set; }
        public bool ForClient { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ComplaintTypeGetDTO? ComplaintType { get; set; }
    }
}
