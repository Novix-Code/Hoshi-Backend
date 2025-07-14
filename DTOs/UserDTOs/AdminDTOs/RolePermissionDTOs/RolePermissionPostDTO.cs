using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RolePermissionDTOs
{
    public class RolePermissionPostDTO 
    {
        public required int RoleId { get; set; }

        public required int PermissionId { get; set; }
    }
}
