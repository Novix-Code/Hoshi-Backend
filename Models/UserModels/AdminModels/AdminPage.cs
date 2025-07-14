using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    [UseFSPController]
    public class AdminPage : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PageId { get; set; }
        public Page? Page { get; set; }
    }
}
