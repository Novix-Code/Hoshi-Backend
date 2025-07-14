using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs
{
    public class PagePostDTO 
    {
        public required string PageName { get; set; }
        public required bool IsDeleted { get; set; }

        public required int ParentPageId { get; set; }
    }
}
