using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionPageDTOs
{
    public class PermissionPagePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? PageId { get; set; }

        public int? PermissionId { get; set; }
    }
}
