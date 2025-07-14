using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    public class RolePermission : TimestampedModel
    {
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
