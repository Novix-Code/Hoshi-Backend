using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels.AdminModels
{
    [EndpointGroupping("Admin")]
    [Index(nameof(PageId), nameof(PermissionId), IsUnique = true)]
    public class PermissionPage : TimestampedModel
    {
        public int PageId { get; set; }
        public Page? Page { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
