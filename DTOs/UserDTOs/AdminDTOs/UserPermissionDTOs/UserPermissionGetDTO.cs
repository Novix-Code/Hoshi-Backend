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
    
    public class AdminWithRolesAndPermissionsDTO
    {
        public UserGetDTO UserGetDto { get; set; }
        public List<RoleWithPermissionsDTO> Roles { get; set; }
    }
    public class RoleWithPermissionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionGetDTO> Permissions { get; set; }
    }

}
