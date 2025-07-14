using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RolePermissionDTOs
{
    public class RolePermissionPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? RoleId { get; set; }

        public int? PermissionId { get; set; }
    }
}
