using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintDTOs
{
    public class ComplaintGetDTO : TimestampedModel, ISoftDelete
    {
        public string Description { get; set; }
        public string? Response { get; set; }
        public bool IsDeleted { get; set; }

        public string ComplaintStatus { get; set; } = Enums.ComplaintStatus.Waitting.ToString();

        public ComplaintTypeGetDTO? ComplaintType { get; set; }

        public OrderGetDTO? Order { get; set; }

        public UserGetDTO? User { get; set; }
    }
}
