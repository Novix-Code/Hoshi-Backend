using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.UserNotificationDTOs
{
    public class UserNotificationPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Description { get; set; }

        public int? UserId { get; set; }

        public int? NotificationTypeId { get; set; }
    }
}
