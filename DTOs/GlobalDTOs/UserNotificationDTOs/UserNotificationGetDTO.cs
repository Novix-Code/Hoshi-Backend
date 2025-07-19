using Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs
{
    public class UserNotificationGetDTO : TimestampedModel
    {
        public string Description { get; set; }

        public UserGetDTO? User { get; set; }

        public NotificationTypeGetDTO? NotificationType { get; set; }
    }
}
