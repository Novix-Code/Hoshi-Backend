using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs
{
    public class NotificationTypeGetDTO: TimestampedModel
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public bool ForClient { get; set; }
    }
}
