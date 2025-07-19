using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserCollectionAlertDTOs
{
    public class UserCollectionAlertPostDTO 
    {
        public required int AlertDays { get; set; }
        public required bool IsRemoved { get; set; }
        public required DateTime RemovingDate { get; set; }

        public required int UserId { get; set; }
    }
}
