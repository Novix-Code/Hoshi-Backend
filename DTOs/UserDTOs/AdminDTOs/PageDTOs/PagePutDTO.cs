using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs
{
    public class PagePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? PageName { get; set; }
        public bool? IsDeleted { get; set; }

        public int? ParentPageId { get; set; }
    }
}
