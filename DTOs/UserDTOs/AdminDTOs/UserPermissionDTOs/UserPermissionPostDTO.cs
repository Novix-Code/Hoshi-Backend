using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs
{
    public class UserPermissionPostDTO 
    {
        public required int UserId { get; set; }

        public required int PermissionId { get; set; }
    }
}
