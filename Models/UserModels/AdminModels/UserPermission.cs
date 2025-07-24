using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels.AdminModels
{
    [EndpointGroupping("Admin")]
    [Index(nameof(UserId), nameof(PermissionId), IsUnique = true)]
    public class UserPermission : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
