using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    public class UserCollectionAlert : TimestampedModel
    {
        public int AlertDays { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime RemovingDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
