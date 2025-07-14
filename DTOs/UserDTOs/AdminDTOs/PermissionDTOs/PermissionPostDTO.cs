using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs
{
    public class PermissionPostDTO 
    {
        public required string PermissionName { get; set; }
        public required bool IsDeleted { get; set; }
    }
}
