using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs
{
    public class PermissionPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? PermissionName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
