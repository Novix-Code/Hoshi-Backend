using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs
{
    public class NotificationTypePostDTO
    {
        public required string Title { get; set; }
        public required string Type { get; set; }
        public required bool ForClient { get; set; }
    }
}
