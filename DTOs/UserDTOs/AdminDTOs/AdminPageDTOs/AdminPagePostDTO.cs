using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs
{
    public class AdminPagePostDTO 
    {
        public required int UserId { get; set; }

        public required int PageId { get; set; }
    }
}
