using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    [EndpointGroupping("Admin")]
    public class PermissionPage : TimestampedModel
    {
        public int PageId { get; set; }
        public Page? Page { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
