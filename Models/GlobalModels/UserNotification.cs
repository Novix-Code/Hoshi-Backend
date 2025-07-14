using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    public class UserNotification : TimestampedModel
    {
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
