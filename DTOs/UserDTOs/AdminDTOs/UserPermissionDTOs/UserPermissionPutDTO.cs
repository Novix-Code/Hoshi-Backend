using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs
{
    public class UserPermissionPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? UserId { get; set; }

        public int? PermissionId { get; set; }
    }
}
