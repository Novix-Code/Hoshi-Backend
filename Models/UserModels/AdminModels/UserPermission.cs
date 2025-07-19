using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    [EndpointGroupping("Admin")]
    public class UserPermission : TimestampedModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
