using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionPageDTOs
{
    public class PermissionPagePostDTO 
    {
        public required int PageId { get; set; }

        public required int PermissionId { get; set; }
    }
}
