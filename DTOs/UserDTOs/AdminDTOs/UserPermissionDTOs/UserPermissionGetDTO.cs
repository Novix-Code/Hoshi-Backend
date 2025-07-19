using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs
{
    public class UserPermissionGetDTO : TimestampedModel
    {
        public UserGetDTO? User { get; set; }

        public PermissionGetDTO? Permission { get; set; }
    }
}
