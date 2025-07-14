using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    public class PermissionPage : TimestampedModel
    {
        public int PageId { get; set; }
        public Page? Page { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
