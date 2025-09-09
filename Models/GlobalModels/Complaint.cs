using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    public class Complaint : TimestampedModel, ISoftDelete
    {
        public string Description { get; set; } = string.Empty;
        public string? Response { get; set; }
        public bool IsDeleted { get; set; }

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public string ComplaintStatus { get; set; } = Enums.ComplaintStatus.Waitting.ToString();

        public int ComplaintTypeId { get; set; }
        public ComplaintType? ComplaintType { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
