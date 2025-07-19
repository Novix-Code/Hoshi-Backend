using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RolePermissionDTOs
{
    public class RolePermissionGetDTO : TimestampedModel
    {
        public RoleGetDTO? Role { get; set; }

        public PermissionGetDTO? Permission { get; set; }
    }
}
