using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs
{
    public class PermissionGetDTO : TimestampedModel, ISoftDelete
    {
        public string PermissionName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
