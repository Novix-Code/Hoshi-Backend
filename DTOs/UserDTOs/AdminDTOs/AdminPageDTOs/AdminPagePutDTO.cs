using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs
{
    public class AdminPagePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? UserId { get; set; }

        public int? PageId { get; set; }
    }
}
