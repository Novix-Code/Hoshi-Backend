using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    [EndpointGroupping("Admin", ControllerAction.Delete)]
    public class UserNotification : TimestampedModel
    {
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType? NotificationType { get; set; }
    }
}
