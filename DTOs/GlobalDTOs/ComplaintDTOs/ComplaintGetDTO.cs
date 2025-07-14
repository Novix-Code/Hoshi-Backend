using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintDTOs
{
    public class ComplaintGetDTO : TimestampedModel, ISoftDelete
    {
        public string Description { get; set; }
        public string? Response { get; set; }
        public bool IsDeleted { get; set; }

        public ComplaintStatus ComplaintStatus { get; set; } = ComplaintStatus.Waitting;

        public ComplaintTypeGetDTO? ComplaintType { get; set; }

        public OrderGetDTO? Order { get; set; }

        public UserGetDTO? User { get; set; }
    }
}
