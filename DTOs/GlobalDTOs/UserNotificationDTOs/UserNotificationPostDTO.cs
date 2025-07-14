using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs
{
    public class UserNotificationPostDTO 
    {
        public required string Description { get; set; }

        public required int UserId { get; set; }

        public required int NotificationTypeId { get; set; }
    }
}
